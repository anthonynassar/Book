using System;
using Android.Content;
using Android.Runtime;
using Xamarin.Forms.Platform.Android;
using Android.Util;
using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using PeopleApp.Droid.Renderers;

[assembly: ExportRenderer(typeof(TagEntry), typeof(TagEntryRenderer))]
namespace PeopleApp.Droid.Renderers
{
    public class TagEntryRenderer : EntryRenderer
    {
        public static void Init()
        {
            new TagEntryRenderer();
        }

        public TagEntryRenderer()
        {
        }

        public TagEntryRenderer(Context context) : base()
        {
        }

        public TagEntryRenderer(Context context, IAttributeSet attrs) : base()
        {
        }

        public TagEntryRenderer(Context context, IAttributeSet attrs, int defStyle) : base()
        {
        }
        public TagEntryRenderer(IntPtr a, JniHandleOwnership b) : base()
        {
        }
    }
}