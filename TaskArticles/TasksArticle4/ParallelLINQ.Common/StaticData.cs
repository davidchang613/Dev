using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelLINQ.Common
{
    #region Data Classes
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
        public ContactDetails ContactDetails { get; set; }

        public override string ToString()
        {
            return String.Format("Name {0}, Age {1}, Email {2}", Name, Age, ContactDetails.Email);
        }


    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
    }

    public class ContactDetails
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    #endregion


    public static class StaticData
    {
        private static Random rand = new Random();

        public static Lazy<int[]> DummyRandomIntValues = new Lazy<int[]>(() =>
            {
                int[] randomInts = new int[30];

                Parallel.For(0, randomInts.Length, (x) =>
                {
                    randomInts[x] = rand.Next(2, 500);
                });

                return (from x in randomInts orderby x ascending select x).ToArray();
            });


        public static Lazy<int[]> DummyRandomHugeIntValues = new Lazy<int[]>(() =>
        {
            int[] randomInts = new int[1000000];

            Parallel.For(0, randomInts.Length, (x) =>
            {
                randomInts[x] = rand.Next(2, 50000);
            });

            return randomInts;
        });



        public static Lazy<int[]> DummyRandomMediumIntValues = new Lazy<int[]>(() =>
        {
            int[] randomInts = new int[10000];

            Parallel.For(0, randomInts.Length, (x) =>
            {
                randomInts[x] = rand.Next(2, 50000);
            });

            return randomInts;
        });


        public static Lazy<int[]> DummyOrderedIntValues = new Lazy<int[]>(() =>
        {
            int[] ints = new int[10];

            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = i;
            }
            return ints;
        });

        public static Lazy<int[]> DummyOrderedLotsOfIntValues = new Lazy<int[]>(() =>
        {
            int[] ints = new int[10000];

            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = i;
            }
            return ints;
        });





        public static Lazy<List<Person>> DummyRandomPeople = new Lazy<List<Person>>(() =>
            {
                List<Person> people = new List<Person>();
                for (int i = 0; i < 150; i++)
                {
                    int offset = (i + 1);
                    Person person = new Person()
                    {
                        Age = offset,
                        Name = string.Format("Person{0}", offset.ToString()),
                        ContactDetails = new ContactDetails
                        {
                            Email = string.Format("Email{0}@{0}.com", offset.ToString()),
                            PhoneNumber = string.Format("{0}{1}{2}{3}{4}{5}",
                            offset.ToString(), (i + 1).ToString(), (i + 2).ToString(),
                            (i + 3).ToString(), (i + 4).ToString(), (i + 5).ToString())
                        },
                        Address = new Address
                        {
                            AddressLine1 = string.Format("Address{0}", offset.ToString()),
                            AddressLine2 = string.Format("Address{0}", (i + 1).ToString()),
                            AddressLine3 = string.Format("Address{0}", (i + 2).ToString()),
                        }

                    };
                    people.Add(person);
                }
                return people;
            });




        public static IEnumerable<Person> DummyRandomPeopleEnumerable()
        {
            for (int i = 0; i < 150; i++)
            {
                int offset = (i + 1);
                Person person = new Person()
                {
                    Age = offset,
                    Name = string.Format("Person{0}", offset.ToString()),
                    ContactDetails = new ContactDetails
                    {
                        Email = string.Format("Email{0}@{0}.com", offset.ToString()),
                        PhoneNumber = string.Format("{0}{1}{2}{3}{4}{5}",
                        offset.ToString(), (i + 1).ToString(), (i + 2).ToString(),
                        (i + 3).ToString(), (i + 4).ToString(), (i + 5).ToString())
                    },
                    Address = new Address
                    {
                        AddressLine1 = string.Format("Address{0}", offset.ToString()),
                        AddressLine2 = string.Format("Address{0}", (i + 1).ToString()),
                        AddressLine3 = string.Format("Address{0}", (i + 2).ToString()),
                    }

                };

                yield return person;
            }
        }





    }


}
