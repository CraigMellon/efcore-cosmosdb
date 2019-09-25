using System;
using System.Collections.Generic;
using System.Linq;
using efcore_cosmosdb.Models;

namespace efcore_cosmosdb
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the EFCore Cosmos DB Provider...");

            CreateJobWithEmbeddedAddress();
            // CreateJobWithEmbeddedContactsCollection()
            // CreateJobWithLinkedResource();
            // CreateMultipleJobsLinkedToSameResource();
        }

        private static void CreateJobWithEmbeddedAddress()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Address = new Address
                {
                    Line1 = "21 Some Street",
                    Line2 = "Somewhere",
                    Town = "Birmingham",
                    PostCode = "B90 4SS",
                }
            };

            using (var context = new JobContext())
            {
                context.Database.EnsureCreated();

                context.Add(job);

                context.SaveChanges();
            }

            using (var context = new JobContext())
            {
                var loadedJob = context.Jobs.First(x => x.Id == job.Id);

                Console.WriteLine($"Job created and retreived with address: {job.Address.Line1}, {job.Address.PostCode}");
            }
        }

        private static void CreateJobWithEmbeddedContactsCollection()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Address = new Address
                {
                    Line1 = "21 Some Street",
                    Line2 = "Somewhere",
                    Town = "Birmingham",
                    PostCode = "B90 4SS",
                },
                Contacts = new List<Contact>()
                {
                    new Contact { Title = "Mr", FirstName = "Craig", LastName = "Mellon", TelephoneNumber = "34441234" },
                    new Contact { Title = "Mrs", FirstName = "Cara", LastName = "Mellon", TelephoneNumber = "53665554" }
                },
                AssignedResource = new Resource
                {
                    Id = Guid.NewGuid(),
                    Title = "Mr",
                    FirstName = "Bob",
                    LastName = "Builder",
                    TelephoneNumber = "0800 1234567"
                }
            };

            using (var context = new JobContext())
            {
                context.Database.EnsureCreated();

                context.Add(job);

                context.SaveChanges();
            }

            using (var context = new JobContext())
            {
                var loadedJob = context.Jobs.First(x => x.Id == job.Id);

                Console.WriteLine($"Job created and retreived with address: {job.Address.Line1}, {job.Address.PostCode}");
                Console.WriteLine($"  Contacts ({job.Contacts.Count()})");
                job.Contacts.ForEach(x =>
                {
                    Console.WriteLine($"    Name: {x.FirstName} {x.LastName}");
                });
            }
        }

        private static void CreateJobWithLinkedResource()
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Address = new Address
                {
                    Line1 = "21 Some Street",
                    Line2 = "Somewhere",
                    Town = "Birmingham",
                    PostCode = "B90 4SS",
                },
                Contacts = new List<Contact>()
                {
                    new Contact { Title = "Mr", FirstName = "Craig", LastName = "Mellon", TelephoneNumber = "34441234" },
                    new Contact { Title = "Mrs", FirstName = "Cara", LastName = "Mellon", TelephoneNumber = "53665554" }
                },
                AssignedResource = new Resource
                {
                    Id = Guid.NewGuid(),
                    Title = "Mr",
                    FirstName = "Bob",
                    LastName = "Builder",
                    TelephoneNumber = "0800 1234567"
                }
            };

            using (var context = new JobContext())
            {
                context.Database.EnsureCreated();

                context.Add(job);

                context.SaveChanges();
            }

            using (var context = new JobContext())
            {
                var loadedJob = context.Jobs.First(x => x.Id == job.Id);

                // now load the resource and assign it to the Job
                var resource = context.Resources.First(x => x.Id == loadedJob.AssignedResourceId);
                loadedJob.AssignedResource = resource;

                Console.WriteLine($"Job created and retreived with address: {job.Address.Line1}, {job.Address.PostCode}");
                Console.WriteLine($"  Contacts ({job.Contacts.Count()})");
                job.Contacts.ForEach(x =>
                {
                    Console.WriteLine($"    Name: {x.FirstName} {x.LastName}");
                });

                Console.WriteLine($"  Assigned Resource: {loadedJob.AssignedResource?.FirstName} {loadedJob.AssignedResource?.LastName}");
            }
        }

        private static void CreateMultipleJobsLinkedToSameResource()
        {
            var resourceId = Guid.NewGuid();

            var resource = new Resource
            {
                Id = resourceId,
                Title = "Mr",
                FirstName = "Bob",
                LastName = "Builder",
                TelephoneNumber = "0800 1234567"
            };

            var job1 = new Job
            {
                Id = Guid.NewGuid(),
                Address = new Address
                {
                    Line1 = "Job 1 Address"
                },
                AssignedResource = resource
            };

            var job2 = new Job
            {
                Id = Guid.NewGuid(),
                Address = new Address
                {
                    Line1 = "Job 2 Address"
                },
                AssignedResource = resource
            };

            using (var context = new JobContext())
            {
                context.Database.EnsureCreated();

                context.Add(job1);
                context.Add(job2);

                context.SaveChanges();
            }

            using (var context = new JobContext())
            {
                var loadedResource = context.Resources.First(x => x.Id == resourceId);

                // Load all jobs with the same assigned resource id
                var jobs = context.Jobs.Where(x => x.AssignedResourceId == resourceId).ToList();

                jobs.ForEach(job =>
                {
                    Console.WriteLine($"Job: {job.Id} - Resource: {job.AssignedResource?.FirstName} {job.AssignedResource?.FirstName}");
                });
            }
        }
    }
}
