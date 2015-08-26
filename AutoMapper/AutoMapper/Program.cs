using System;

namespace AutoMapper
{
    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Position { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public int YearsInCompany { get; set; }
        public DateTime StartDate { get; set; }

        public Employee Build()
        {
            return new Employee
            {
                Name = "John Smith",
                Email = "john@email.net",
                Address = new Address
                {
                    Country = "USA",
                    City = "New York",
                    Street = "Wall Street",
                    Number = 7
                },
                Position = "Manager",
                Gender = true,
                Age = 35,
                YearsInCompany = 5,
                StartDate = new DateTime(2007, 11, 2)
            };
        }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
    }

    public class EmployeeViewItem
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int YearsInCompany { get; set; }
        public string StartDate { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var Employee = new Employee().Build();

            Mapper.CreateMap<Employee, EmployeeViewItem>()
               .ForMember(dest => dest.Address,
                           source => source.MapFrom(employee => employee.Address.City + ", " +
                                                                employee.Address.Street + " " +
                                                                employee.Address.Number))
               .BeforeMap((source, dest) => source.Email = source.Email.ToUpper())
               .AfterMap((source, dest) => dest.Age = dest.Age + 10)
               .ForMember(dest => dest.Position, source => source.Condition(src => (src.Position.Equals(""))))
               .ForMember(dest => dest.Gender,
                           source => source.ResolveUsing<GenderResolver>().FromMember(e => e.Gender))
               .ForMember(dest => dest.StartDate, source => source.AddFormatter<DateFormatter>())
               .ForMember(dest => dest.Name, source => source.Ignore());

            EmployeeViewItem employeeVIewItem = Mapper.Map<Employee, EmployeeViewItem>(Employee);

            Console.Write(employeeVIewItem);
        }

        public class GenderResolver : ValueResolver<bool, string>
        {
            protected override string ResolveCore(bool gender)
            {
                return gender ? "Male" : "Female";
            }
        }

        public class DateFormatter : IValueFormatter
        {
            public string FormatValue(ResolutionContext context)
            {
                return ((DateTime)context.SourceValue).ToShortDateString();
            }
        }
    }
}
