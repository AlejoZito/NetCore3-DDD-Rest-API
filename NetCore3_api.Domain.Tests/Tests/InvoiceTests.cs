using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Contracts.Exceptions;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Models.ValueObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Tests.Tests
{
    public class InvoiceTests
    {
        const string TEST_USERNAME = "Donatello";

        [Test]
        public void GetValidInvoiceTotalAmount()
        {
            Invoice invoice = new Invoice()
            {
                Currency = Enumerations.Currency.ARS,
                Month = 4,
                Year = 2019,
                User = GetTestUser()
            };
            Assert.DoesNotThrow(() =>
            {
                //Add to valid april 2019 charges
                invoice.AddCharge(GetTestCharge(new DateTime(2019, 4, 01)));
                invoice.AddCharge(GetTestCharge(new DateTime(2019, 4, 23)));
            });

            var totalInvoiceAmount = invoice.GetAmountCurrency();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(totalInvoiceAmount, "Invoice must have a total amount and it is null");
                Assert.AreEqual(200, totalInvoiceAmount.Amount, "Invoice total must be $200");
                Assert.AreEqual(Currency.ARS, totalInvoiceAmount.Currency, "Invoice total currency must be ARS");
                Assert.AreEqual(Currency.ARS, invoice.Currency, "Invoice currency must be ARS");
            });
        }

        [Test]
        public void CannotAddInvalidCharges()
        {
            Invoice invoice = new Invoice()
            {
                Currency = Enumerations.Currency.ARS,
                Month = 4,
                Year = 2019,
                User = GetTestUser()
            };
            Assert.Throws(typeof(InvalidChargeException),() =>
            {
                //Add valid april 2019 charges
                invoice.AddCharge(GetTestCharge(new DateTime(2019, 4, 01)));
                //Add invalid 2018 charge
                invoice.AddCharge(GetTestCharge(new DateTime(2018, 4, 23)));
            }, "Cannot add charge with invalid Date");

            Assert.Throws(typeof(InvalidChargeException), () =>
            {
                //Add valid april 2019 charges
                invoice.AddCharge(GetTestCharge(new DateTime(2019, 4, 01)));
                //Add invalid 2018 charge
                invoice.AddCharge(GetTestCharge(new DateTime(2019, 4, 23), new AmountCurrency(500, "US")));
            }, "Cannot add charge with invalid Currency");
        }

        private Charge GetTestCharge(DateTime? date = null, AmountCurrency amountCurrency = null)
        {
            return new Charge()
            {
                Amount = amountCurrency ?? new AmountCurrency(100, "ARS"),
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
