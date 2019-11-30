using Moq;
using NetCore3_api.Domain.Contracts;
using NetCore3_api.Domain.DomainServices;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using NetCore3_api.Domain.Models.ValueObjects;
using NetCore3_api.Domain.Models.Aggregates.Payment;
using NetCore3_api.Domain.Tests.Helpers;

namespace NetCore3_api.Domain.Tests.Tests
{
    public class UserDebtServiceTests
    {
        [Test]
        public async Task ShouldGetCorrectDebt()
        {
            var chargeRepositoryMock = new Mock<IRepository<Charge>>();
            chargeRepositoryMock.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Charge, bool>>>(),
                It.IsAny<SortOptions>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
                .Returns((Expression<Func<Charge,bool>> predicate, SortOptions sortOptions, int? pageSize, int? pageNum)=>
                //Return charges after applying UserDebtService's predicate
                Task.FromResult(GetTestCharges().Where(predicate.Compile()).ToList()));

            UserDebtService userDebtService = new UserDebtService(chargeRepositoryMock.Object);

            AmountCurrency debt = await userDebtService.GetUserDebt(
                MockData.GetTestUser().Id, Enumerations.Currency.ARS);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(63.75M, debt.Amount, "AR$ debt should be $63.75");
                Assert.AreEqual(Enumerations.Currency.ARS, debt.Currency, "Debt currency should be AR$");
            });
        }

        [Test]
        public async Task PaymentShouldBeValidForUserDebt()
        {
            var chargeRepositoryMock = new Mock<IRepository<Charge>>();
            chargeRepositoryMock.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Charge, bool>>>(),
                It.IsAny<SortOptions>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
                .Returns((Expression<Func<Charge, bool>> predicate, SortOptions sortOptions, int? pageSize, int? pageNum) =>
                //Return charges after applying UserDebtService's predicate
                Task.FromResult(GetTestCharges().Where(predicate.Compile()).ToList()));

            UserDebtService userDebtService = new UserDebtService(chargeRepositoryMock.Object);

            bool isValidPayment = await userDebtService.IsValidPayment(
                new Payment(63.75M, Enumerations.Currency.ARS, MockData.GetTestUser()));

            Assert.IsTrue(isValidPayment, "A payment of 63.75 AR$ should be valid for a user with a 63.75 AR$ debt");
        }

        [Test]
        public async Task PaymentShouldNotBeValidForUserDebt()
        {
            var chargeRepositoryMock = new Mock<IRepository<Charge>>();
            chargeRepositoryMock.Setup(x => x.ListAsync(
                It.IsAny<Expression<Func<Charge, bool>>>(),
                It.IsAny<SortOptions>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
                .Returns((Expression<Func<Charge, bool>> predicate, SortOptions sortOptions, int? pageSize, int? pageNum) =>
                //Return charges after applying UserDebtService's predicate
                Task.FromResult(GetTestCharges().Where(predicate.Compile()).ToList()));

            UserDebtService userDebtService = new UserDebtService(chargeRepositoryMock.Object);

            bool isValidPayment = await userDebtService.IsValidPayment(
                new Payment(100, Enumerations.Currency.ARS, MockData.GetTestUser()));

            Assert.IsFalse(isValidPayment, "A payment of 100 AR$ should not be valid because it exceeds the 63.75 AR$ user's debt.");
        }

        /// <summary>
        /// Returns 4 charges.
        /// 1) AR$ 150 fully paid
        /// 2) AR$ 200 with 140$ paid and 60$ remaining
        /// 3) AR$ 3.75 unpaid
        /// 4) U$ 100 unpaid
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Charge> GetTestCharges()
        {
            User testUser = MockData.GetTestUser();

            return new List<Charge>()
            {
                //Fully paid charge
                new Charge(){
                    Id = 1,
                    Amount = new AmountCurrency(150, Enumerations.Currency.ARS),
                    Payments = new List<PaymentCharge>()
                    {
                        new PaymentCharge(null, new Payment(100, Enumerations.Currency.ARS, testUser), 100),
                        new PaymentCharge(null, new Payment(50, Enumerations.Currency.ARS, testUser), 50)
                    },
                    Event = new Event(){Date = new DateTime(2019, 1,1), User = testUser } },
                //Partially paid charge (200 - 50 - 90 = 60)
                new Charge(){
                    Id = 2,
                    Amount = new AmountCurrency(200, Enumerations.Currency.ARS),
                    Payments = new List<PaymentCharge>()
                    {
                        new PaymentCharge(null, new Payment(50, Enumerations.Currency.ARS, testUser), 50),
                        new PaymentCharge(null, new Payment(90, Enumerations.Currency.ARS, testUser), 90)
                    },
                    Event = new Event(){Date = new DateTime(2019, 1,5), User = testUser } },
                //Unpaid charge (3.75)
                new Charge(){
                    Id = 3,
                    Amount = new AmountCurrency(3.75m, Enumerations.Currency.ARS),
                    Payments = new List<PaymentCharge>()
                    {
                    },
                    Event = new Event(){Date = new DateTime(2019, 1,5), User = testUser }
                },
                //Unpaid charge 100 U$
                new Charge(){
                    Id = 3,
                    Amount = new AmountCurrency(100, Enumerations.Currency.US),
                    Payments = new List<PaymentCharge>()
                    {
                    },
                    Event = new Event(){Date = new DateTime(2019, 1,5), User = testUser }
                },
            };
        }
    }
}
