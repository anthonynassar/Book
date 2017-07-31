using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
#if Android
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
#endif
#if __IOS__
using Foundation;
using UIKit;
using CoreFoundation;
using ImageIO;
#endif
using System.Xml;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace PeopleApp.Core
{
    class MetaExtractor
    {
        public string path { get; set; }

        public MetaExtractor(string path)
        {
            this.path = path;
        }

        public string Extract()
        {
            //MetaExtractor metaExtractor = new MetaExtractor(path);
            return produceXML(this.path);
        }

        private string produceXML(string path)
        {
            // Extract the metadata
            List<PhotoModel> photosMeta = extractMetadata(path);
            // Create the XML file
            return writeXML(photosMeta);
        }

        private static List<PhotoModel> extractMetadata(string path)
        {
            List<PhotoModel> photoLocations = new List<PhotoModel>();
            // reading from directory
            string[] files = System.IO.Directory.GetFiles(path, "*.jp*g");
            foreach (var item in files)
            {
                Console.WriteLine(item.ToString());
            }
            if (files.Length == 0)
            {
                Console.Error.WriteLine("No matching files found.");
                Environment.Exit(1);
            }

            
            PhotoModel ds = new PhotoModel();
            

            // extracting metadata for each file
            foreach (string file in files)
            {
                Console.WriteLine("_________________________ NEW PHOTO _________________________");
#if __IOS__

                var url = new NSUrl(file, false);  // could be an NSUrl to asset lib...
                CGImageSource myImageSource;
                myImageSource = CGImageSource.FromUrl(url, null);
                var ns = new NSDictionary();
                var imageProperties = myImageSource.CopyProperties(ns, 0);
                var width = imageProperties[CGImageProperties.PixelWidth];
                var height = imageProperties[CGImageProperties.PixelHeight];
                Console.WriteLine("Dimensions: {0}x{1}", width, height);

                // for debugging output all the image metadata
                Console.WriteLine(imageProperties.DescriptionInStringsFileFormat);
                hm.Clear();

                // tiff
                var tiff = imageProperties.ObjectForKey(CGImageProperties.TIFFDictionary) as NSDictionary;
                var artist = tiff[CGImageProperties.TIFFArtist];
                var make = tiff[CGImageProperties.TIFFMake];
                var model = tiff[CGImageProperties.TIFFModel];
                if (artist != null)
                    hm.Add("owner", artist.ToString());
                else
                    hm.Add("owner", make + " " + model);

                // exif
                var exif = imageProperties.ObjectForKey(CGImageProperties.ExifAuxDictionary) as NSDictionary;
                var dateTimeOriginal = tiff[CGImageProperties.ExifDateTimeOriginal];
                if (dateTimeOriginal != null)
                    hm.Add("date", dateTimeOriginal.ToString());
                else
                    hm.Add("date", "");

                // gps
                var gps = imageProperties.ObjectForKey(CGImageProperties.GPSDictionary) as NSDictionary;
                var lat = gps[CGImageProperties.GPSLatitude];
                var latref = gps[CGImageProperties.GPSLatitudeRef];
                var lon = gps[CGImageProperties.GPSLongitude];
                var lonref = gps[CGImageProperties.GPSLongitudeRef];
                var loc = String.Format("GPS: {0} {1}, {2} {3}", lat, latref, lon, lonref);
                hm.Add("lat", lat.ToString());
                hm.Add("lng", lon.ToString());
                Console.WriteLine(loc);
                string coordinate = lat + "," + lon;
                ReverseGeocode(hm, coordinate);
                
                photoLocations.Add(new PhotoModel(Double.Parse(lat.ToString()), Double.Parse(lon.ToString()), file, hm));
#endif

#if Android
                photoLocations = ExtractMetadataPerPhoto(file);
#endif
            }
            return photoLocations;
        }

        public static List<PhotoModel> ExtractMetadataPerPhoto(string file)
        {
            Dictionary<string, string> hm = new Dictionary<string, string>();
            List<PhotoModel> photoLocations = new List<PhotoModel>();

            // read all metadata from the image
            Stream stream = File.OpenRead(file);

            var directories = ImageMetadataReader.ReadMetadata(stream);

            stream.Close();
            // interrogate exifIFD0 meta

            var exifIfd0Directory = directories.OfType<ExifIfd0Directory>();
            foreach (var item in exifIfd0Directory)
            {
                //string model = exifIfd0Directory.
                string model = item?.GetDescription(ExifIfd0Directory.TagModel);
                string make = item?.GetDescription(ExifIfd0Directory.TagMake);
                string artist = item?.GetDescription(ExifIfd0Directory.TagArtist);
                hm.Clear();

                if (!String.IsNullOrEmpty(artist))
                {
                    hm.Add("owner", artist);
                }
                else
                {
                    hm.Add("owner", make + " " + model);
                }
            }
            // interrogate exifIFD0 meta

            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>();

            foreach (var item in subIfdDirectory)
            {
                var dateTime = item?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
                if (dateTime != null)
                {
                    hm.Add("date", dateTime.ToString());
                }
                else
                {
                    hm.Add("date", "");
                }

            }
            // See whether it has GPS data
            var gpsDirectories = directories.OfType<GpsDirectory>();
            foreach (var item in gpsDirectories)
            {
                GeoLocation geoLocation = item.GetGeoLocation();
                if (geoLocation != null && !geoLocation.IsZero)
                {
                    // reverse geocoding processing
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); // This is used to have a decimal point rather than comma in decimal numbers
                    hm.Add("premise", "");
                    hm.Add("streetNumber", "");
                    hm.Add("route", "");
                    hm.Add("locality", "");
                    hm.Add("sublocality", "");
                    hm.Add("adminArea", "");
                    hm.Add("adminArea3", "");
                    hm.Add("country", "");
                    hm.Add("postalCode", "");
                    string coordinate = geoLocation.ToString();
                    string[] latlng = coordinate.Split(',');
                    hm.Add("lat", latlng[0]);
                    hm.Add("lng", latlng[1].TrimStart());

                    ReverseGeocode(hm, coordinate);
                    // Add to our collection for use below
                    photoLocations.Add(new PhotoModel(file, hm));

                    break; // get out of the for loop
                }
            }
            return photoLocations;
        }

        private static void ReverseGeocode(Dictionary<string, string> hm, string coordinate)
        {
            var address = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + coordinate + "&key=AIzaSyB7bADUszXp-UbX-m8twdgfE6yntChXroM";
            // Download the data from google API
            System.Net.WebClient webClient = new System.Net.WebClient();
            var rawData = webClient.DownloadData(address);
            var encoding = System.Text.Encoding.UTF8;       // The encoding used in t he downloaded data
            var result = encoding.GetString(rawData);
            //var result = new System.Net.WebClient().DownloadString(address);
            GoogleGeoCodeResponse test = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result.ToString());

            //test.results[1].address_components.

            foreach (var adComponent in test.results[0].address_components)
            {
                foreach (var componentType in adComponent.types)
                {
                    switch (componentType)
                    {
                        case "premise":
                            hm["premise"] = adComponent.long_name;
                            break;
                        case "street_number":
                            hm["streetNumber"] = adComponent.long_name;
                            break;
                        case "route":
                            hm["route"] = adComponent.long_name;
                            break;
                        case "locality":
                            hm["locality"] = adComponent.long_name;
                            break;
                        case "sublocality":
                            hm["sublocality"] = adComponent.long_name;
                            break;
                        case "administrative_area_level_1":
                            hm["adminArea"] = adComponent.long_name;
                            break;
                        case "country":
                            hm["country"] = adComponent.long_name;
                            break;
                        case "postal_code":
                            hm["postalCode"] = adComponent.long_name;
                            break;
                        default:
                            break;
                    }
                }
            }
            foreach (var adComponent in test.results[1].address_components)
            {
                foreach (var componentType in adComponent.types)
                {
                    if (componentType.Equals("administrative_area_level_3"))
                        hm["adminArea3"] = adComponent.long_name;
                }
            }
            hm.Add("street", "");
            hm.Add("cityCP", ""); //department
            hm.Add("department", ""); //stateCP

            // add city
            if (!hm["locality"].Equals(""))
            {
                hm["cityCP"] = hm["locality"] + " " + hm["postalCode"];
            }
            else if (!hm["sublocality"].Equals(""))
            {
                hm["cityCP"] = hm["sublocality"] + " " + hm["postalCode"];
            }
            else
            {
                hm["cityCP"] = hm["adminArea3"] + " " + hm["postalCode"];
            }

            // add premise instead of route if the latter does not exist to street key
            if (hm["route"].Equals(""))
            {
                hm["street"] = hm["premise"];
            }
            else
            {
                hm["street"] = hm["streetNumber"] + " " + hm["route"];
            }

            // add department plus postal code
            hm["department"] = hm["adminArea"];
        }

        private static string writeXML(List<PhotoModel> photosMeta)
        {
            XmlDocument doc = new XmlDocument();
            // Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";

            // create root node
            XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("tbl_IMG_META"));
            //photosMeta.Add(1); photosMeta.Add(2); photosMeta.Add(3);
            foreach (var photo in photosMeta)
            {
                // create imgMeta node
                XmlElement imgMeta = doc.CreateElement("IMG_META");
                root.AppendChild(imgMeta);
                imgMeta.SetAttribute("id_image", Path.GetFileNameWithoutExtension(photo.file));

                // create metaName node
                createElement(imgMeta, doc, "1", photo.map["owner"]);
                createElement(imgMeta, doc, "2", photo.map["lat"]);
                createElement(imgMeta, doc, "3", photo.map["lng"]);
                createElement(imgMeta, doc, "4", photo.map["country"]);
                createElement(imgMeta, doc, "5", photo.map["cityCP"]);
                createElement(imgMeta, doc, "6", photo.map["department"]);
                createElement(imgMeta, doc, "7", photo.map["street"]);
                createElement(imgMeta, doc, "8", photo.map["date"]);
                createElement(imgMeta, doc, "9", "keywords");

                root.AppendChild(imgMeta);
            }
            doc.AppendChild(root);
            doc.InsertBefore(xmldecl, root);

            // Media for XML data output
            //doc.Save("foo1.xml");
            Console.WriteLine(doc.OuterXml);
            StringBuilder sb = new StringBuilder();
            System.IO.TextWriter tr = new System.IO.StringWriter(sb);
            XmlTextWriter wr = new XmlTextWriter(tr);
            wr.Formatting = System.Xml.Formatting.Indented;
            doc.Save(wr);
            wr.Close();
            return sb.ToString();
        }

        private static void createElement(XmlElement imgMeta, XmlDocument doc, string idMeta, string value)
        {
            XmlElement metaName = doc.CreateElement("META");
            metaName.SetAttribute("id_meta", idMeta);
            XmlText metaValue = doc.CreateTextNode(value);
            metaName.AppendChild(metaValue);
            imgMeta.AppendChild(metaName);
        }
    }

    class PhotoModel
    {
        //public readonly GeoLocation location;
        //public double latitude;
        //public double longitude;
        public readonly string file;
        public readonly Dictionary<string, string> map;

        public PhotoModel()
        {
        }

        //public Photo(GeoLocation location, string file, Dictionary<string, string> map)
        //{
        //    this.map = new Dictionary<string, string>(map);
        //    this.location = location;
        //    this.file = file;
        //}
        public PhotoModel(string file, Dictionary<string, string> map)
        {
            this.map = new Dictionary<string, string>(map);
            //this.latitude = latitude;
            //this.longitude = longitude;
            this.file = file;
        }

    }
}
