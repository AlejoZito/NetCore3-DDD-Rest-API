using Moq;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.DomainServices;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Domain.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using NetCore3_api.Domain.Models.ValueObjects;
using NetCore3_api.Domain.Enumerations;
using NetCore3_api.Domain.Contracts.Exceptions;

namespace NetCore3_api.Domain.Tests.Tests
{
    public class PaymentServiceTests
    {
        [Test]
        public async Task PaymentShouldFulfilCompletelyTwoCharges()
        {
            var testCharges = new List<Charge>(){
                    new Charge(){
                        Id = 1,
                        Amount = new AmountCurrency(150, Enumerations.Currency.ARS),
                        Event = new Event(){Date = new DateTime(2019, 1,1) } },
                    new Charge(){
                        Id = 2,
                        Amount = new AmountCurrency(50, Enumerations.Currency.ARS),
                    Event = new Event(){Date = new DateTime(2019, 1,2) } },
                    new Charge(){
                        Id = 3,
                        Amount = new AmountCurrency(10, Enumerations.Currency.ARS),
                        Event = new Event(){Date = new DateTime(2019, 1,3) }
                    }
            };

            var chargeRepository = new Mock<IRepository<Charge>>();
            chargeRepository.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Charge, bool>>>(),
                It.IsAny<SortOptions>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
                .Returns(Task.FromResult(testCharges.OrderBy(x => x.Event.Date).ToList()));

            //Setup mock to return "inserted" entity
            var paymentRepository = new Mock<IRepository<Payment>>();
            paymentRepository.Setup(x => x.Insert(It.IsAny<Payment>()))
                    .Returns((Payment p) => p);

            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.FindByIdAsync(It.IsAny<long>()))
                    .Returns(Task.FromResult(MockData.GetTestUser()));

            var userDebtService = new Mock<IUserDebtService>();
            userDebtService.Setup(x => x.IsValidPayment(It.IsAny<Payment>()))
                    .Returns(Task.FromResult(true));
            //Setup userdebt service to always return test charges as unpaid charges
            userDebtService.Setup(x => x.GetUnpaidCharges(It.IsAny<long>(), It.IsAny<Currency>()))
                    .Returns(Task.FromResult(testCharges.OrderBy(x => x.Event.Date).ToList()));

            var invoiceService = new Mock<IInvoiceService>();

            PaymentService paymentService = new PaymentService(
                paymentRepository.Object,
                userRepository.Object,
                userDebtService.Object,
                invoiceService.Object);

            Payment createdPayment = await paymentService.ExecutePayment(200, "ARS", 1);

            Assert.IsNotNull(createdPayment.Charges);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, createdPayment.Charges.Count, "Payment should be linked to 2 charges");
                Assert.AreEqual(1, createdPayment.Charges.ElementAt(0).Charge.Id, "Payment should be linked with charge with ID 1");
                Assert.AreEqual(150, createdPayment.Charges.ElementAt(0).Amount, "Payment to charge 1 should fulfil full amount.");
                Assert.AreEqual(2, createdPayment.Charges.ElementAt(1).Charge.Id, "Payment should be linked with charge with ID 2");
                Assert.AreEqual(50, createdPayment.Charges.ElementAt(1).Amount, "Payment to charge 2 should fulfil full amount.");
                Assert.AreEqual(MockData.TEST_USERNAME, createdPayment.User.Username, "Payment should be linked to test user");
            });
        }

        /// <summary>
        /// If user has 2 charges with AR$ 150 and AR$200 and executes a $200 payment:
        /// The first charge will be fully paid, and the second one will be partially paid.
        /// Oldest charges are prioritized and paid first.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task PaymentShouldFulfilPartiallyTwoCharges()
        {
            var testCharges = new List<Charge>(){
                    new Charge(){
                        Id = 1,
                        Amount = new AmountCurrency(150, Enumerations.Currency.ARS),
                        Event = new Event(){Date = new DateTime(2019, 1,1) } },
                    new Charge(){
                        Id = 2,
                        Amount = new AmountCurrency(200, Enumerations.Currency.ARS),
                        Event = new Event(){Date = new DateTime(2019, 2,1) }
                    }
            };

            var chargeRepository = new Mock<IRepository<Charge>>();
            chargeRepository.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Charge, bool>>>(),
                It.IsAny<SortOptions>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
                .Returns(Task.FromResult(testCharges.OrderBy(x => x.Event.Date).ToList()));

            //Setup mock to return "inserted" entity
            var paymentRepository = new Mock<IRepository<Payment>>();
            paymentRepository.Setup(x => x.Insert(It.IsAny<Payment>()))
                .Returns((Payment p) => p);

            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.FindByIdAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(MockData.GetTestUser()));

            var userDebtService = new Mock<IUserDebtService>();
            userDebtService.Setup(x => x.IsValidPayment(It.IsAny<Payment>()))
                .Returns(Task.FromResult(true));
            //Setup userdebt service to always return test charges as unpaid charges
            userDebtService.Setup(x => x.GetUnpaidCharges(It.IsAny<long>(), It.IsAny<Currency>()))
                    .Returns(Task.FromResult(testCharges.OrderBy(x => x.Event.Date).ToList()));

            var invoiceService = new Mock<IInvoiceService>();

            PaymentService paymentService = new PaymentService(
                paymentRepository.Object,
                userRepository.Object,
                userDebtService.Object,
                invoiceService.Object);

            Payment createdPayment = await paymentService.ExecutePayment(200, "ARS", 1);

            Assert.IsNotNull(createdPayment.Charges);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, createdPayment.Charges.Count, "Payment should be linked to 2 charges");
                Assert.AreEqual(1, createdPayment.Charges.ElementAt(0).Charge.Id, "Payment should be linked with charge with ID 1");
                Assert.AreEqual(150, createdPayment.Charges.ElementAt(0).Amount, "Payment to charge 1 should fulfil full amount.");
                Assert.AreEqual(2, createdPayment.Charges.ElementAt(1).Charge.Id, "Payment should be linked with charge with ID 2");
                Assert.AreEqual(50, createdPayment.Charges.ElementAt(1).Amount, "Payment to charge 2 should only assign 50 AR$.");
                Assert.AreEqual(MockData.TEST_USERNAME, createdPayment.User.Username, "Payment should be linked to test user");
            });
        }

        [Test]
        public async Task ShouldAllowPaymentsUntilDebtIsFulfilled()
        {
            var testCharge = new Charge()
            {
                Id = 1,
                Amount = new AmountCurrency(100, Enumerations.Currency.ARS),
                Event = new Event() { Date = new DateTime(2019, 1, 1) },
                Payments = new List<PaymentCharge>()
            };

            var chargeRepository = new Mock<IRepository<Charge>>();
            chargeRepository.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Charge, bool>>>(),
                It.IsAny<SortOptions>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
                .Returns(Task.FromResult(new List<Charge>() { testCharge }));

            //Setup mock to return "inserted" entity
            var paymentRepository = new Mock<IRepository<Payment>>();
            paymentRepository.Setup(x => x.Insert(It.IsAny<Payment>()))
                    .Returns((Payment p) => p);

            var userRepository = new Mock<IRepository<User>>();
            userRepository.Setup(x => x.FindByIdAsync(It.IsAny<long>()))
                    .Returns(Task.FromResult(MockData.GetTestUser()));

            var userDebtService = new Mock<IUserDebtService>();
            //Setup debt service to return valid payment if the payment does not exceed the charge's unpaid amount
            userDebtService.Setup(x => x.IsValidPayment(It.IsAny<Payment>()))
                    .Returns((Payment p)=>Task.FromResult(p.Amount <= testCharge.GetUnPaidAmount().Amount));
            //Setup debt service to always return charge variable
            userDebtService.Setup(x => x.GetUnpaidCharges(It.IsAny<long>(), It.IsAny<Currency>()))
                    .Returns(Task.FromResult(new List<Charge>() { testCharge }));
            
            var invoiceService = new Mock<IInvoiceService>();

            PaymentService paymentService = new PaymentService(
                paymentRepository.Object,
                userRepository.Object,
                userDebtService.Object,
                invoiceService.Object);

            Payment firstPayment = await paymentService.ExecutePayment(50, "ARS", 1);
            testCharge.Payments.Add(new PaymentCharge(testCharge, firstPayment, 50));

            Payment secondPayment = await paymentService.ExecutePayment(40, "ARS", 1);
            testCharge.Payments.Add(new PaymentCharge(testCharge, firstPayment, 40));

            Assert.CatchAsync(typeof(InvalidEntityException), async () =>
             {
                 Payment thirdPayment = await paymentService.ExecutePayment(20, "ARS", 1); //this payment exceeds 100AR$ debt and will be rejected
            });
        }
    }
}
