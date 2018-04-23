using DevExpress.DevAV;
using DevExpress.Mvvm.Native;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Collections.ObjectModel;

namespace DevExpress.DevAV.Generator {
    public class DatabaseGenerator {
        class Picture {
            public byte[] Data { get; set; }
        }

        SqlConnection sqlConnection;
        List<Picture> malePictures;
        List<Picture> femalePictures;
        List<Picture> currentMalePictures;
        List<Picture> currentFemalePictures;
        public void Init() {
            RaiseProgress("Loading Photos...");
            string dbPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Data\Generator\DevAV_source.mdf");
            string connectionString = string.Format("Server=(localdb)\\mssqllocaldb;Integrated Security=true;database=DevAV_source;AttachDbFileName={0}", dbPath);
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        void RaiseProgress(string p) {
            Console.WriteLine(p);
        }

        public void Seed(DevAVDb context) {
            Init();

            string connectionString = context.Database.Connection.ConnectionString;

            var allResources = typeof(DatabaseGenerator).Assembly.GetManifestResourceNames();
            malePictures = allResources.Where(r => r.Contains("MalePhotos")).Select(name => new Picture { Data = ReadEmbeddedResource(name) }).ToList();
            femalePictures = allResources.Where(r => r.Contains("FemalePhotos")).Select(name => new Picture { Data = ReadEmbeddedResource(name) }).ToList();
            ResetPictures();

            context.SaveChanges();

            context.SaveChanges();
            var crests = CreateCrests();
            context.SaveChanges();
            var stateEnums = CreateStateEnums();
            context.SaveChanges();
            var employees = CreateEmployees(stateEnums);
            context.Employees.AddRange(employees.Values);
            FixEmployeeStatus(employees.Values.ToList());
            context.SaveChanges();
            var products = CreateProducts(employees);
            context.Products.AddRange(products.Values);
            context.SaveChanges();
            var customers = CreateCustomers(stateEnums);
            PatchCustomers(customers.Values.ToList());
            context.Customers.AddRange(customers.Values);
            context.SaveChanges();
            var customerStores = CreateCustomerStores(stateEnums, crests, customers);
            context.CustomerStores.AddRange(customerStores.Values);
            context.SaveChanges();
            currentMalePictures = malePictures.ToList();
            currentFemalePictures = femalePictures.ToList();
            var tasks = CreateTasks(employees/*, customerEmployees*/);
            context.Tasks.AddRange(tasks.Values);
            context.SaveChanges();
            context.SaveChanges();
            var orders = CreateOrders(employees, customers, customerStores);
            context.Orders.AddRange(orders.Values);
            context.SaveChanges();
            context.SaveChanges();
            context.SaveChanges();
            context.SaveChanges();
            context.SaveChanges();
            context.SaveChanges();
            RaiseProgress("Updating locations...");
            context.SaveChanges();
            var newOrders = new List<Order>();
            DuplicateOrders(context, customers, customerStores, orders.Values.ToList(), connectionString, newOrders);
            CreateFakeQuotes(context, connectionString, newOrders);
            DropMigrationHistory(context);
        }

        void DropMigrationHistory(DevAVDb context) {
            using (var command = context.Database.Connection.CreateCommand()) {
                context.Database.Connection.Open();
                command.CommandText = "drop table __MigrationHistory";
                command.ExecuteNonQuery();
            }
        }

        static string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam risus turpis, placerat vel leo ut, ullamcorper varius nisi. Donec facilisis diam ut nunc consectetur, ut fringilla metus auctor. Maecenas rhoncus augue sit amet arcu ullamcorper malesuada. Cras mollis diam in tortor posuere suscipit. Mauris aliquam justo vitae nulla viverra malesuada.\n"
            + "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis varius diam non aliquam tincidunt. Suspendisse non accumsan dui, quis fermentum elit. Proin in risus accumsan.\n"
            + "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi ornare metus nec tellus suscipit, vitae.";


        void ResetPictures() {
            currentMalePictures = malePictures.ToList();
            currentFemalePictures = femalePictures.ToList();
        }

        void FixEmployeeStatus(List<Employee> employees) {
            var salaried = employees.Where(e => e.Status == EmployeeStatus.Salaried).ToList();
            salaried[4].Status = EmployeeStatus.Contract;
            salaried[5].Status = EmployeeStatus.Contract;
            salaried[6].Status = EmployeeStatus.Contract;
            salaried[7].Status = EmployeeStatus.OnLeave;
            salaried[8].Status = EmployeeStatus.OnLeave;
        }

        void PatchCustomers(List<Customer> customers) {
            Customer first = customers.First(c => c.AnnualRevenue > 0);
            Random seed = new Random(13);
            decimal multiplier = 5000000000m;
            var revenues = Enumerable.Range(0, 12).Select(_ => seed.Next(2, 10) * multiplier)
                .Concat(Enumerable.Range(12, 7).Select(_ => seed.Next(11, 100) * multiplier));
            customers.Except(new[] { first }).Zip(revenues, (c, r) => c.AnnualRevenue = r).ToArray();
            customers.ToList().ForEach(c =>
            {
                c.TotalStores = seed.Next(20, 35);
                c.TotalEmployees = seed.Next(2, 20) * 1000;
            });
        }

        Quote OrderToQuote(Order order, ref int number/*, List<OrderItem> orderItems, List<QuoteItem> items*/) {
            var quote = new Quote
            {
                Customer = order.Customer,
                CustomerStore = order.Store,
                Date = order.OrderDate.AddDays(random.Next(-28, -7)),
                Employee = order.Employee,
                Number = (number += 3).ToString(),
                ShippingAmount = order.ShippingAmount,
                SubTotal = order.TotalAmount + order.ShippingAmount,
                Total = order.TotalAmount
            };

            return quote;
        }

        void CreateFakeQuotes(DevAVDb context, string connectionString, /*List<OrderItem> orderItems, */List<Order> allOrders) {
            var newQuotes = new List<Quote>();

            Random seed = new Random(13);
            var percentages = Enum.GetValues(typeof(StateEnum)).Cast<StateEnum>().ToDictionary(s => s, s => seed.NextDouble() / 1.4 + 0.2); // 0.2 .. 0.9
            int number = 3000;
            int curState = 0;
            var stateGroups = allOrders.GroupBy(o => o.Store.State).ToList();
            foreach(var stateGroup in stateGroups) {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                StateEnum state = stateGroup.Key;
                List<Order> orders = stateGroup.ToList();
                var quotes = orders.Select(o =>
                {
                    var quote = OrderToQuote(o, ref number/*, orderItems, items*/);
                    return quote;
                }).ToList();
                while(percentages[state] < (double)orders.Count / quotes.Count) {
                    quotes.Add(OrderToQuote(orders[seed.Next() % orders.Count], ref number/*, orderItems, items*/));
                }
                RaiseProgress(string.Format("Creating quotes for state {0}/{1} ({2})", ++curState, stateGroups.Count, sw.Elapsed));
                UpdateOpportunity(quotes, percentages[state]);
                newQuotes.AddRange(quotes);
            }

            context.Quotes.AddRange(newQuotes);
            context.SaveChanges();
            context.SaveChanges();
        }

        void UpdateOpportunity(List<Quote> quotes, double percentage) {
            Random seed = new Random(27);
            var ps = Enumerable.Range(0, quotes.Count).Select(_ => seed.NextDouble()).ToList();
            double p;
            int i = 0;
            do {
                i++;
                p = ps.Average();
                int idx = seed.Next() % ps.Count;
                if(p > percentage)
                    ps[idx] = Math.Max(ps[idx] - 0.02, 0);
                else
                    ps[idx] = Math.Min(ps[idx] + 0.02, 1);
            } while(Math.Abs(p - percentage) > 0.1);
            ps = ps.Select(p0 =>
            {
                if(Math.Abs(p0 - 1) < 0.01)
                    return seed.NextDouble() / 5.0 + 0.8;
                if(p0 < 0.01)
                    return seed.NextDouble() / 5.0;
                return p0;
            }).ToList();
            quotes.Zip(ps, (q, p1) => q.Opportunity = p1).ToList();
        }

        T Duplicate<T>(T item) where T : DatabaseObject, new() {
            T res = new T();
            foreach(var prop in typeof(T).GetProperties().Where(p => p.Name != "Id")) {
                prop.SetValue(res, prop.GetValue(item, null), null);
            }
            return res;
        }

        void DuplicateOrdersToDate(
            ReadOnlyCollection<Order> ordersForFirstCustomer,
            List<Order> newOrders
            ) {
            int curInvoiceNumber = ordersForFirstCustomer.Max(o => (int)o.InvoiceNumber) + 3;
            int curPONumber = ordersForFirstCustomer.Max(o => int.Parse(o.PONumber)) + 3;
            DateTime minDate = ordersForFirstCustomer.Min(o => o.OrderDate);
            DateTime maxDate = new DateTime(2014, 12, 31);
            TimeSpan span = ordersForFirstCustomer.Max(o => o.OrderDate) - minDate;
            int neededCopy = (int)((maxDate.Ticks - minDate.Ticks) / span.Ticks);
            for(int i = 1;
            i <= neededCopy;
            i++) {
                foreach(Order protoOrder in ordersForFirstCustomer.Where(o => o.OrderDate + span <= maxDate).ToList()) {
                    Order dupOrder = Duplicate(protoOrder);
                    dupOrder.ShipDate = dupOrder.ShipDate.AddTicks(span.Ticks * i);
                    dupOrder.OrderDate = dupOrder.OrderDate.AddTicks(span.Ticks * i);
                    dupOrder.InvoiceNumber = curInvoiceNumber;
                    dupOrder.PONumber = curPONumber.ToString();
                    newOrders.Add(dupOrder);
                    curInvoiceNumber += 3;
                    curPONumber += 3;
                }
            }
        }

        void DuplicateOrders(DevAVDb context,
            Dictionary<int, Customer> customers,
            Dictionary<int, CustomerStore> customerStores,
            List<Order> protoOrders,
            string connectionString,
            List<Order> newOrders) {
            RaiseProgress("duplicating orders (years)");
            Customer firstCustomer = customers[1];
            PreCorrectOrders(context, protoOrders);
            protoOrders.ForEach(o =>
            {
                o.OrderDate += TimeSpan.FromDays(730);
                o.ShipDate += TimeSpan.FromDays(730);
            });
            DuplicateOrdersToDate(protoOrders.AsReadOnly(),
                newOrders
                );

            protoOrders.AddRange(newOrders);

            int curInvoiceNumber = protoOrders.Max(o => (int)o.InvoiceNumber) + 3;
            int curPONumber = protoOrders.Max(o => int.Parse(o.PONumber)) + 3;

            RaiseProgress("duplicating orders (customers)");
            int customersDone = 0;
            List<CustomerStore> firstCustomerStores = customerStores.Values.Where(s => s.Customer.Id == firstCustomer.Id).ToList();
            foreach(Customer customer in customers.Values.Except(new[] { firstCustomer })) {
                List<CustomerStore> stores = customerStores.Values.Where(s => s.Customer.Id == customer.Id).ToList();
                Debug.Assert(stores.Count == firstCustomerStores.Count);
                var storeMap = new Dictionary<CustomerStore, CustomerStore>();
                for(int i = 0;
                i < stores.Count;
                ++i) {
                    storeMap[firstCustomerStores[i]] = stores[i];
                }
                foreach(Order order in protoOrders) {
                    Order dupOrder = Duplicate(order);
                    dupOrder.Customer = customer;
                    dupOrder.Store = storeMap[order.Store];
                    dupOrder.InvoiceNumber = curInvoiceNumber;
                    dupOrder.PONumber = curPONumber.ToString();
                    newOrders.Add(dupOrder);
                    curInvoiceNumber += 3;
                    curPONumber += 3;
                }
                RaiseProgress(string.Format("done {0}/{1}", ++customersDone, customers.Values.Count() - 1));
            }

            context.Orders.AddRange(newOrders);
            context.SaveChanges();
            context.SaveChanges();
            PostCorrectOrders(context);
        }

        void PostCorrectOrders(DevAVDb context) {
            RaiseProgress("PostCorrectOrders");
            var random = new Random();
            Dictionary<Customer, Tuple<int, bool>> correction = CalculateCorrectionsForOrdersAndOrderItems(context, random);
            foreach(var order in context.Orders) {
                CorrectDates(random, order);
                CorrectOrdersAndOrderItems(context, random, correction, order);
            }
            context.SaveChanges();
        }

        static void CorrectOrdersAndOrderItems(DevAVDb context, Random random, Dictionary<Customer, Tuple<int, bool>> correction, Order order) {
            if(order.OrderDate.Year != correction[order.Customer].Item1)
                return;
            bool isNegativeFactor = correction[order.Customer].Item2;
            int factor = random.Next(1, 4);
            if(isNegativeFactor)
                factor *= -1;
            order.SaleAmount = 1000; //orderItems.Sum(oi => oi.Total);
            order.TotalAmount = order.SaleAmount + order.ShippingAmount;
        }

        static Dictionary<Customer, Tuple<int, bool>> CalculateCorrectionsForOrdersAndOrderItems(DevAVDb context, Random random) {
            Dictionary<Customer, Tuple<int, bool>> correction = new Dictionary<Customer, Tuple<int, bool>>();
            int minYear = context.Orders.Min(o => o.OrderDate.Year);
            int maxYear = context.Orders.Max(o => o.OrderDate.Year);
            var customers = context.Customers.ToList();
            for(int i = 0;
            i < customers.Count;
            i++) {
                int year = random.Next(minYear, maxYear + 1);
                correction[customers[i]] = new Tuple<int, bool>(year, random.Next(2) == 0);
            }
            return correction;
        }

        static void CorrectDates(Random random, Order order) {
            var correctSpan = TimeSpan.FromDays(random.Next(0, 6));
            order.OrderDate = order.OrderDate + correctSpan;
            order.ShipDate = order.ShipDate + correctSpan;
        }

        void PreCorrectOrders(DevAVDb context, List<Order> orders) {
            CorrectSpacesInOrders(orders);
        }

        static void CorrectSpacesInOrders(List<Order> orders) {
            DateTime lastDate = orders[0].OrderDate;
            TimeSpan maxSpan = new TimeSpan(7, 0, 0, 0);
            var random = new Random();
            for(int i = 1;
            i < orders.Count;
            i++) {
                var spanForLast = orders[i].OrderDate - lastDate;
                if(spanForLast > maxSpan) {
                    var correctionSpan = spanForLast - maxSpan + TimeSpan.FromDays(random.Next(-1, 1));
                    for(int k = i;
                    k < orders.Count;
                    k++) {
                        orders[k].OrderDate = orders[k].OrderDate - correctionSpan;
                        orders[k].ShipDate = orders[k].ShipDate - correctionSpan;
                    }
                }
                lastDate = orders[i].OrderDate;
            }
        }
        public class Crest : DatabaseObject {
            public string CityName { get; set; }
            public byte[] SmallImage { get; set; }
            public byte[] LargeImage { get; set; }
        }
        public List<Crest> CreateCrests() {
            RaiseProgress("Loading Crests...");
            var map = new Dictionary<string, string> {
                { "Los Angeles", "Seal_of_Los_Angeles,_California.svg" },
                { "Anaheim", "Seal_of_Anaheim,_California.svg" },
                { "San Diego", "Seal_Of_San_Diego,_California.svg" },
                { "San Jose", "Seal_of_San_Jose,_California" },
                { "Las Vegas", "Las_Vegas_seal.svg" },
                { "Reno", "Nevada-StateSeal.svg" },
                { "Denver", "DenverCOseal" },
                { "Casper", "Seal_of_Wyoming.svg" },
                { "Salt Lake City", "Seal_of_Utah.svg" },
                { "Phoenix", "Arizona-StateSeal.svg" },
                { "Seattle", "Corporate_seal_of_the_City_of_Seattle" },
                { "Spokane", "Seal_of_Washington.svg" },
                { "Portland", "Seal_of_Portland_OR" },
                { "Eugene", "Seal_of_Oregon.svg" },
                { "Sacramento", "Seal_of_California.svg" },
                { "Boise", "Seal_of_Idaho.svg" },
                { "Tucson", "Flag_of_Tucson,_Arizona" },
                { "Colorado Springs", "Seal_of_Colorado.svg" },
                { "Albuquerque", "NewMexico-StateSeal.svg" },
                { "Vancouver", "Vancouver-wa-city-logo2" },
            };
            return map.Select(p =>
                new Crest
            {
                CityName = p.Key,
                SmallImage = ReadResource("/Data/Generator/Seals/" + p.Value + ".small.png"),
                LargeImage = ReadResource("/Data/Generator/Seals/" + p.Value + ".large.png")
            }
            ).ToList();
        }

        List<Tuple<string, byte[]>> GetResources(string part1, string part2) {
            var allResources = typeof(DatabaseGenerator).Assembly.GetManifestResourceNames();
            return allResources.Where(r => r.Contains(part1) && r.Contains(part2)).Select(n => Tuple.Create(n, ReadEmbeddedResource(n))).ToList();
        }

        Stack<Picture> GetProductImages(string part1, string part2) {
            return new Stack<Picture>(GetResources(part1, part2).Select(p => new Picture { Data = p.Item2 }));
        }

        T TakeAny<T>(IEnumerable<T> list) where T : class {
            return list.ElementAt(random.Next() % list.Count());
        }

        T TakeAnyAndRemove<T>(List<T> list) where T : class {
            T item = TakeAny(list);
            list.Remove(item);
            return item;
        }

        public Dictionary<int, Order> CreateOrders(
            Dictionary<int, Employee> employees,
            Dictionary<int, Customer> customers,
            Dictionary<int, CustomerStore> customerStores) {
            RaiseProgress("Loading Orders...");
            return GetRows("Orders", "Order_ID", g => new Order
            {
                InvoiceNumber = long.Parse(g.As<string>("Order_Invoice_Number")),
                Customer = customers[g.AsScalar<int>("Order_Customer_ID")],
                Store = customerStores[g.AsScalar<int>("Order_Customer_Location_ID")],
                PONumber = g.As<string>("Order_PO_Number"),
                Employee = employees[g.AsScalar<int>("Order_Employee_ID")],
                OrderDate = g.AsScalar<DateTime>("Order_Date"),
                SaleAmount = g.AsScalar<decimal>("Order_Sale_Amount"),
                ShippingAmount = g.AsScalar<decimal>("Order_Shipping_Amount"),
                TotalAmount = g.AsScalar<decimal>("Order_Total_Amount"),
                ShipDate = g.AsScalar<DateTime>("Order_Ship_Date"),
                OrderTerms = g.As<string>("Order_Terms")
            });
        }

        public static DateTime GetRandomDateForProductMapView() {
            int factor = 0;
            switch(random.Next(0, 3)) {
                case 0:
                    factor = 1;
                    break;
                case 1:
                    factor = 28;
                    break;
                case 2:
                    factor = 360;
                    break;
                case 3:
                    factor = 700;
                    break;
            }
            TimeSpan daySpan = new TimeSpan(24, 0, 0);
            var span = new TimeSpan((long)(daySpan.Ticks * random.NextDouble() * factor));
            return DateTime.Now - span;
        }

        static string FixAddress(string address) {
            var split = address.Split('\n');
            if(split.Length > 1)
                return split.Last();
            return address;
        }

        static string FixCity(string city) {
            return city.Replace("Tuscon", "Tucson")
                .Replace("Colorando", "Colorado")
                .Replace("Alburquerque", "Albuquerque")
                .Replace("Vacouver", "Vancouver");
        }

        public Dictionary<int, CustomerStore> CreateCustomerStores(
            Dictionary<int, StateEnum> states,
            List<Crest> crests,
            Dictionary<int, Customer> customers) {
            return GetRows("Customer_Store_Locations", "Customer_Store_ID", g =>
            {
                var addressCity = FixCity(g.As<string>("Customer_Store_City"));
                return new CustomerStore
                {
                    AddressLine = FixAddress(g.As<string>("Customer_Store_Address")),
                    AddressCity = addressCity,
                    AddressState = states[g.AsScalar<int>("Customer_Store_State")],
                    AddressZipCode = g.As<string>("Customer_Store_Zipcode"),
                    Phone = FixPhone(g.As<string>("Customer_Store_Phone")),
                    Fax = FixPhone(g.As<string>("Customer_Store_Fax")),
                    TotalEmployees = g.AsScalar<int>("Customer_Store_Total_Employees"),
                    SquereFootage = g.AsScalar<int>("Customer_Store_Square_Footage"),
                    AnnualSales = g.AsScalar<decimal>("Customer_Store_Annual_Sales"),
                    CrestLarge = crests.First(c => c.CityName == addressCity).LargeImage,
                    CrestSmall = crests.First(c => c.CityName == addressCity).SmallImage,
                    Customer = customers[g.AsScalar<int>("Customer_ID")],
                    Location = g.As<string>("Customer_Store_Location")
                };
            });
        }

        public Dictionary<int, StateEnum> CreateStateEnums() {
            return GetRows("States", "Sate_ID", g => g.AsEnum<StateEnum>("State_Short"));
        }
        public Dictionary<int, Customer> CreateCustomers(Dictionary<int, StateEnum> states) {
            RaiseProgress("Loading Customers...");
            var logos = GetResources("CustomerLogos", "").Select(l => l.Item2).ToList();
            return GetRows("Customers", "Customer_ID", g => new Customer
            {
                Name = g.As<string>("Customer_Name"),
                AddressLine = FixAddress(g.As<string>("Customer_Billing_Address")),
                AddressCity = FixCity(g.As<string>("Customer_Billing_City")),
                AddressState = states[g.AsScalar<int>("Customer_Billing_State")],
                AddressZipCode = g.As<string>("Customer_Billing_Zipcode"),
                Phone = g.As<string>("Customer_Phone"),
                Fax = g.As<string>("Customer_Fax"),
                Website = g.As<string>("Customer_Website"),
                AnnualRevenue = g.AsScalar<decimal>("Customer_Annual_Revenue"),
                TotalStores = g.AsScalar<int>("Customer_Total_Stores"),
                TotalEmployees = g.AsScalar<int>("Customer_Total_Employees"),
                Status = RowGetter.ParseEnum<CustomerStatus>(g.As<string>("Customer_Status") ?? "Active"),
                Logo = TakeAnyAndRemove(logos),
            });
        }

        DateTime? ParseDateTime(string str) {
            if(str == null)
                return null;
            DateTime res = DateTime.Parse(str, CultureInfo.InvariantCulture);
            if(res.Year == 203)
                res = new DateTime(2013, res.Month, res.Day, res.Hour, res.Minute, res.Second);
            return res;
        }

        string Unescape(string str, bool keepReturns) {
            string noTags = new Regex("<.*?>").Replace(str, "");
            string noEscapes = noTags.Replace("&amp;", "&").Replace("&nbsp;", " ");
            if(keepReturns)
                return noEscapes;
            var split = noEscapes.Split('\n')
                .Where(l => !string.IsNullOrWhiteSpace(l));
            if(split.Any())
                return split.Aggregate((l, r) => l + "\n" + r);
            return "";

        }

        public Dictionary<int, EmployeeTask> CreateTasks(
            Dictionary<int, Employee> employees
            ) {
            RaiseProgress("Loading Tasks...");
            return GetRows("Tasks", "Task_ID", g =>
            {
                DateTime? date = ParseDateTime(g.As<string>("Task_Reminder_Date"));
                DateTime? time = ParseDateTime(g.As<string>("Task_Reminder_Time"));
                int? customerEmployeeId = g.AsNullable<int>("Task_Customer_Employee_ID");
                return new EmployeeTask
                {
                    AssignedEmployee = employees[g.AsScalar<int>("Task_Assigned_Employee_ID")],
                    Subject = g.As<string>("Task_Subject"),
                    Description = Unescape(g.As<string>("Task_Description"), false),
                    StartDate = ParseDateTime(g.As<string>("Task_Start_Date")),
                    DueDate = ParseDateTime(g.As<string>("Task_Due_Date")),
                    Status = g.AsEnum<EmployeeTaskStatus>("Task_Status"),
                    Priority = g.AsEnum<EmployeeTaskPriority>("Task_Priority"),
                    Completion = g.AsNullable<int>("Task_Completion") ?? 0,
                    Reminder = g.AsScalar<bool>("Task_Reminder"),
                    ReminderDateTime = (date == null || time == null) ? (DateTime?)null :
                        new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, time.Value.Hour, time.Value.Minute, time.Value.Second)
                };
            });
        }

        static Random random = new Random();
        public Dictionary<int, Employee> CreateEmployees(
            Dictionary<int, StateEnum> states
            //, Dictionary<int, Probation> probations
            ) {
            RaiseProgress("Loading Employees...");
            return GetRows("Employees", "Employee_ID", g =>
            {
                var profile = Unescape(g.As<string>("Employee_Personal_Profile") ?? "", true);
                var id = g.AsScalar<int>("Employee_ID");
                if(id == 48) { // Brad Farkus
                    profile = "Brad is on probation for excessive tardiness.  We hope to see him back at his desk shortly.\n\nPlease remember tardiness is not something we tolerate.";
                }
                if(id == 22) { // Amelia Harper
                    profile = "Amelia is on probation for failure to follow-up on tasks.  We hope to see her back at her desk shortly.\n\nPlease remember negligence of assigned tasks is not something we tolerate.";
                }
                var prefix = ParsePrefix(g.As<string>("Employee_Prefix"));
                string fullName = g.As<string>("Employee_Full_Name");
                Employee res = new Employee
                {
                    FirstName = g.As<string>("Employee_First_Name"),
                    LastName = g.As<string>("Employee_Last_Name"),
                    Prefix = prefix,
                    Title = g.As<string>("Employee_Title"),
                    Picture = GetImageForPrefix(prefix, fullName).With(x => x.Data),
                    AddressLine = FixAddress(g.As<string>("Employee_Address")),
                    AddressCity = FixCity(g.As<string>("Employee_City")),
                    AddressState = states[g.AsScalar<int>("Employee_State_ID")],
                    AddressZipCode = g.AsScalar<int>("Employee_Zipcode").ToString("d5"),
                    Email = g.As<string>("Employee_Email"),
                    Skype = g.As<string>("Employee_Skype"),
                    MobilePhone = FixPhone(g.As<string>("Employee_Mobile_Phone")),
                    HomePhone = FixPhone(g.As<string>("Employee_Home_Phone")),
                    BirthDate = g.AsScalar<DateTime>("Employee_Birth_Date"),
                    HireDate = g.AsScalar<DateTime>("Employee_Hire_Date"),
                    Department = (EmployeeDepartment)g.AsScalar<int>("Employee_Department_ID"),
                    Status = g.AsEnum<EmployeeStatus>("Employee_Status"),
                    PersonalProfile = profile,
                    Order = g.AsScalar<int>("Employee_ID")
                };
                return res;
            });
        }

        string FixPhone(string num) {
            num = num.Replace(" ", "").Replace(")", "").Replace("(", "").Replace("-", "");
            return string.Format("({0}) {1}-{2}", num.Substring(0, 3), num.Substring(3, 3), num.Substring(6, 4));
        }
        public Dictionary<int, Product> CreateProducts(Dictionary<int, Employee> employees) {
            RaiseProgress("Loading Products...");
            return GetRows("Products", "Product_ID", g => new Product
            {
                Name = g.As<string>("Product_Name"),
                Description = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                ProductionStart = g.AsScalar<DateTime>("Product_Production_Start"),
                Available = g.AsScalar<bool>("Product_Available"),
                Image = ReadResource("/Data/Generator/Products/ProductImage.png"),
                Support = employees[g.AsScalar<int>("Product_Support_ID")],
                Engineer = employees[g.AsScalar<int>("Product_Engineer_ID")],
                CurrentInventory = g.AsNullable<int>("Product_Current_Inventory"),
                Backorder = g.AsScalar<int>("Product_Backorder"),
                Manufacturing = g.AsScalar<int>("Product_Manufacturing"),
                Barcode = g.As<byte[]>("Product_Barcode"),
                Cost = g.AsScalar<decimal>("Product_Cost"),
                SalePrice = g.AsScalar<decimal>("Product_Sale_Price"),
                RetailPrice = g.AsScalar<decimal>("Product_Retail_Price"),
                ConsumerRating = g.AsScalar<double>("Product_Consumer_Rating"),
                Category = GetCategory(g.As<string>("Product_Category"))
            });
        }

        class RowGetter {
            DataRow row;
            public RowGetter(DataRow row) {
                this.row = row;
            }
            public T As<T>(string name) where T : class {
                var val = row[name];
                if(val is DBNull)
                    return null;
                if(typeof(T) == typeof(string))
                    return val.ToString() as T;
                return (T)val;
            }
            public T? AsNullable<T>(string name) where T : struct {
                var val = row[name];
                if(val is DBNull)
                    return null;
                return (T)val;
            }
            public T AsScalar<T>(string name) {
                return (T)row[name];
            }
            public T AsEnum<T>(string name) {
                return ParseEnum<T>((string)row[name]);
            }
            public static T ParseEnum<T>(string str) {
                return (T)Enum.Parse(typeof(T), str.Replace(" ", ""));
            }
        }

        Dictionary<int, T> GetRows<T>(string tableName, string idName, Func<RowGetter, T> selector) {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.TableMappings.Add("Table", tableName);
            adapter.SelectCommand = new SqlCommand(string.Format("SELECT * FROM dbo.{0};", tableName), sqlConnection) { CommandType = CommandType.Text };
            DataSet dataSet = new DataSet(tableName);
            adapter.Fill(dataSet);
            return dataSet.Tables[tableName].Rows.Cast<DataRow>()
                .ToDictionary(row => (int)row[idName], row => selector(new RowGetter(row)));
        }

        static string GetNoteText(int i) {
            switch(i) {
                case 0:
                    return "This is an RTF note for Jimmy. Jimmy is one of those great people. He is understanding.";
                case 1:
                    return "Jimmy's birthday today. He turns 25. Don't forget to join us for the celebration.";
                default:
                    return "random note " + i;
            }
        }
        Picture GetImageForPrefix(PersonPrefix prefix, string fullName) {
            Picture res = null;
            if(fullName == "Samantha Bright") {
                prefix = PersonPrefix.Ms;
            } else if(fullName == "Ed Holmes") {
                prefix = PersonPrefix.Mr;
            } else if(fullName == "Ken Samuelson") {
                prefix = PersonPrefix.Mr;
            } else if(fullName == "Billy Zimmer") {
                return null;
            }
            if(prefix == PersonPrefix.Mr) {
                res = currentMalePictures[random.Next(0, currentMalePictures.Count - 1)];
                currentMalePictures.Remove(res);
            } else if(prefix == PersonPrefix.Ms || prefix == PersonPrefix.Mrs || prefix == PersonPrefix.Miss) {
                res = currentFemalePictures[random.Next(0, currentFemalePictures.Count - 1)];
                currentFemalePictures.Remove(res);
            }
            return res; // Dr
        }

        PersonPrefix ParsePrefix(string p) {
            switch(p) {
                default:
                    return PersonPrefix.Miss;
                case "Dr.":
                    return PersonPrefix.Dr;
                case "Miss":
                    return PersonPrefix.Miss;
                case "Mr.":
                    return PersonPrefix.Mr;
                case "Mrs.":
                    return PersonPrefix.Mrs;
                case "Ms.":
                    return PersonPrefix.Ms;
            }
        }

        byte[] ReadResource(string relativePath) {
            relativePath = "pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component" + relativePath;
            using (BinaryReader binaryReader = new BinaryReader(Application.GetResourceStream(new Uri(relativePath)).Stream)) {
                return binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
            }
        }
        byte[] ReadEmbeddedResource(string name) {
            name = name.Replace("%20", " ");
            using (BinaryReader binaryReader = new BinaryReader(typeof(DatabaseGenerator).Assembly.GetManifestResourceStream(name))) {
                return binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
            }
        }
        ProductCategory GetCategory(string value) {
            ProductCategory res;
            if(Enum.TryParse(value, out res))
                return res;
            return ProductCategory.VideoPlayers;
        }
    }
}