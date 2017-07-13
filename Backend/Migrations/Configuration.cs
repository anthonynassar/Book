namespace Backend.Migrations
{
    using Backend.DataObjects;
    using Backend.Models;
    using Microsoft.Azure.Mobile.Server.Tables;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Backend.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
            ContextKey = "Backend.Models.MobileServiceContext";
        }

        protected override void Seed(MobileServiceContext context)
        {
            //This method will be called after migrating to the latest version.

            //You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //to avoid creating duplicate seed data.

            //List<Dimension> dimensions = new List<Dimension>
            //{
            //    new Dimension { Id = "Time", Interval = true },
            //    new Dimension { Id = "Location", Interval = true },
            //    new Dimension { Id = "Topic", Interval = true },
            //    new Dimension { Id = "Social", Interval = false }
            //};
            //context.Set<Dimension>().AddRange(dimensions);

            //var constId1 = NewGuid();
            //var constId2 = NewGuid();
            //List<Constraint> constraints = new List<Constraint>
            //{
            //    new Constraint { Id = constId1, Operator = "less", Value="2" },
            //    new Constraint { Id = constId2, Operator = "more", Value="4" }
            //};
            //context.Set<Constraint>().AddRange(constraints);

            //var userId1 = NewGuid();
            //var userId2 = NewGuid();
            //List<User> users = new List<User>
            //{
            //    new User { Id = userId1, Name = "Person A", Address = 64600, Email = "some@email.com", Profession = "Student", Privilege = "Admin" },
            //    new User { Id = userId2, Name = "Person B", Address = 64600, Email = "some@email.com", Profession = "Student", Privilege = "Admin" }
            //};
            //context.Set<User>().AddRange(users);

            //var ssId1 = NewGuid();
            //var ssId2 = NewGuid();
            //List<SharingSpace> sharingSpaces = new List<SharingSpace>
            //{
            //    new SharingSpace { Id = ssId1, CreationLocation = "Anglet", Descriptor = "A new sharing space", UserId = userId1 },
            //    new SharingSpace { Id = ssId2, CreationLocation = "Anglet", Descriptor = "A new sharing space", UserId = userId2 },
            //};
            //context.Set<SharingSpace>().AddRange(sharingSpaces);

            //List<Event> events = new List<Event>
            //{
            //    new Event {  ConstraintId = constId1, DimensionId = "Time", SharingSpaceId = ssId1},
            //    new Event {  ConstraintId = constId2, DimensionId = "Location", SharingSpaceId = ssId1},
            //    new Event {  ConstraintId = constId1, DimensionId = "Topic", SharingSpaceId = ssId1},
            //    new Event {  ConstraintId = constId1, DimensionId = "Time", SharingSpaceId = ssId2},
            //    new Event {  ConstraintId = constId1, DimensionId = "Location", SharingSpaceId = ssId2}
            //};
            //context.Set<Event>().AddRange(events);

            //List<TodoItem> todoItems = new List<TodoItem>
            //{
            //    new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
            //    new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false }
            //};

            //foreach (TodoItem todoItem in todoItems)
            //{
            //    context.Set<TodoItem>().Add(todoItem);
            //}

            base.Seed(context);
        }

        private string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
