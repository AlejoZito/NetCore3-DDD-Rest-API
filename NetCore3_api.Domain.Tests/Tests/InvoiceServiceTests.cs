using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.DomainServices;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Models.ValueObjects;
using NetCore3_api.Domain.Tests.Helpers;
using NUnit.Framework;
namespace NetCore3_api.Domain.Tests
{
    public class InvoiceServiceTests
    {
        const string TEST_USERNAME = "Oscar Capristo";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task MustGetValidInvoiceAmountsAndCurrencies()
        {
            var invoiceRepositoryMock = new Mock<IRepository<Invoice>>();

            //Return value passed to repository when calling INSERT
            invoiceRepositoryMock
                .Setup((x => x.Insert(It.IsAny<Invoice>())))
                .Returns((Invoice createdInvoice) => { return createdInvoice; });

            InvoiceService invoiceService = new InvoiceService(invoiceRepositoryMock.Object);

            //Add a charge with date in april 2019.
            Invoice newInvoice = await invoiceService.AddCharge(GetTestCharge(new DateTime(2019, 4, 1)), GetTestUser());

            Assert.IsNotNull(newInvoice, "Must create a new invoice for user in April 2019");
            if (newInvoice != null)
            {
                var totalInvoiceAmount = newInvoice.GetAmountCurrency();
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(totalInvoiceAmount, "Invoice must have a total amount and it is null");
                    Assert.AreEqual(100, totalInvoiceAmount.Amount, "Invoice total must be $200");
                    Assert.AreEqual(Currency.ARS, totalInvoiceAmount.Currency, "Invoice total currency must be ARS");
                    Assert.AreEqual(Currency.ARS, newInvoice.Currency, "Invoice currency must be ARS");
                });
            }
        }

        [Test]
        public async Task MustQueryCorrectInvoices()
        {
            var testUser = MockData.GetTestUser();
            var testInvoices = new List<Invoice>()
            {
                new Invoice(11, 2018, Currency.ARS, testUser),
                new Invoice(12, 2018, Currency.ARS, testUser),
                new Invoice(01, 2019, Currency.ARS, testUser),
                new Invoice(01, 2019, Currency.US, testUser),
                new Invoice(02, 2019, Currency.ARS, testUser),
                new Invoice(03, 2019, Currency.ARS, testUser),
            };
            
            var invoiceRepositoryMock = new Mock<IRepository<Invoice>>();

            //Return value passed to repository when calling INSERT
            invoiceRepositoryMock.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Invoice, bool>>>(),
                It.IsAny<SortOptions>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
                .Returns((Expression<Func<Invoice, bool>> predicate, SortOptions sortOptions, int? pageSize, int? pageNum) =>
                //Return invoices after applying InvoiceService's predicate
                Task.FromResult(testInvoices.Where(predicate.Compile()).ToList()));

            InvoiceService invoiceService = new InvoiceService(invoiceRepositoryMock.Object);

            List<Invoice> invoicesResult = await invoiceService.GetInvoicesForUser(
                userId: testUser.Id,
                fromYear: 2018,
                fromMonth: 12,
                toYear: 2019,
                toMonth: 2);

            Assert.AreEqual(4, invoicesResult.Count, "Query should return 4 invoices");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(12, invoicesResult[0].Month, "First invoice should be December 2018");
                Assert.AreEqual(2018, invoicesResult[0].Year, "First invoice should be December 2018");
                Assert.AreEqual(01, invoicesResult[1].Month, "Second invoice should be January 2019");
                Assert.AreEqual(2019, invoicesResult[1].Year, "Second invoice should be January 2019");
                Assert.AreEqual(01, invoicesResult[2].Month, "Third invoice should be January 2019");
                Assert.AreEqual(2019, invoicesResult[2].Year, "Third invoice should be January 2019");
                Assert.AreEqual(Currency.US, invoicesResult[2].Currency, "Third invoice should be in U$ currency");
                Assert.AreEqual(02, invoicesResult[3].Month, "Fourth invoice should be February 2019");
                Assert.AreEqual(2019, invoicesResult[3].Year, "Fourth invoice should be February 2019");
            });           
        }

        //ToDo: mover a infrastructure/integration tests
        //[Test]
        //public async Task Test1()
        //{
        //    var invoiceRepositoryMock = new Mock<IRepository<Invoice>>();

        //    //ToDo Setup mock

        //    InvoiceService invoiceService = new InvoiceService(invoiceRepositoryMock.Object);

        //    //Add 2 charges with dates in april 2019.
        //    await invoiceService.AddCharge(DateTime.Now, GetTestCharge(new DateTime(2019, 4, 1)), GetTestUser());
        //    await invoiceService.AddCharge(DateTime.Now, GetTestCharge(new DateTime(2019, 4, 21)), GetTestUser());
        //    //Add charge for another period
        //    await invoiceService.AddCharge(DateTime.Now, GetTestCharge(new DateTime(2018, 4, 1)), GetTestUser());

        //    var invoice = await invoiceService.GetInvoiceForUser(GetTestUser().Id, 4, 2019);

        //    Assert.IsNotNull(invoice, "Must create and retrieve invoice for user in April 2019");
        //    if (invoice != null)
        //    {
        //        Assert.Multiple(() =>
        //        {
        //            Assert.AreEqual(TEST_USERNAME, invoice.User.Username, "The invoice username must match the charge username");
        //            Assert.AreEqual(2, invoice.Charges.Count, "Invoice must have 2 charges");
        //            Assert.AreEqual(200, invoice.Charges.Count, "Invoice total must be ARS 200");
        //        });
        //    }
        //}


        //[Test]
        //public async Task MustGetValidInvoiceAmountsAndCurrencies()
        //{
        //    var invoiceRepositoryMock = new Mock<IRepository<Invoice>>();

        //    //ToDo Setup mock

        //    InvoiceService invoiceService = new InvoiceService(invoiceRepositoryMock.Object);

        //    //Add 2 charges with dates in april 2019.
        //    await invoiceService.AddCharge(DateTime.Now, GetTestCharge(new DateTime(2019, 4, 1)), GetTestUser());
        //    await invoiceService.AddCharge(DateTime.Now, GetTestCharge(new DateTime(2019, 4, 21)), GetTestUser());
        //    //Add charge for another period
        //    await invoiceService.AddCharge(DateTime.Now, GetTestCharge(new DateTime(2018, 4, 1)), GetTestUser());

        //    var invoice = await invoiceService.GetInvoiceForUser(GetTestUser().Id, 4, 2019);

        //    Assert.IsNotNull(invoice, "Must create and retrieve invoice for user in April 2019");
        //    if (invoice != null)
        //    {
        //        var totalInvoiceAmount = invoice.GetAmountCurrency();
        //        Assert.Multiple(() =>
        //        {
        //            Assert.IsNotNull(totalInvoiceAmount, "Invoice must have a total amount and it is null");
        //            Assert.AreEqual(200, totalInvoiceAmount.Amount, "Invoice total must be $200");
        //            Assert.AreEqual("ARS", totalInvoiceAmount.Currency, "Invoice total currency must be ARS");
        //            Assert.AreEqual(Currency.ARS, invoice.Currency, "Invoice currency must be ARS");
        //        });
        //    }
        //}

        private Charge GetTestCharge(DateTime? date = null)
        {
            return new Charge()
            {
                Amount = new AmountCurrency(100, "ARS"),
                Event = new Event()
                {
                    Id = 1,
                    Date = date ?? DateTime.Now,
                    Type = new EventType() { },
                    User = GetTestUser(),
                },
                Payments = new List<PaymentCharge>()
            };
        }

        public User GetTestUser()
        {
            return new User()
            {
                Id = 1,
                Username = TEST_USERNAME
            };
        }
    }
}