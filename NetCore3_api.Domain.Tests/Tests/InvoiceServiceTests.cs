using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.DomainServices;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Models.ValueObjects;
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