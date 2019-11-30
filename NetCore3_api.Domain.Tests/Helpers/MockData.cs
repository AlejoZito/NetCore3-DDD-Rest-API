using NetCore3_api.Domain.Models.Aggregates.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Domain.Tests.Helpers
{
    public class MockData
    {
        public const string TEST_USERNAME = "Donatello";
        
        public static User GetTestUser()
        {
            return new User()
            {
                Id = 1,
                Username = TEST_USERNAME
            };
        }
    }
}
