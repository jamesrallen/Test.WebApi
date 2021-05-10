// Test.WebApi/Test.WebApi/TestWebApiUser.cs
// James Allen
// 2021/05/10/2:42 PM

using Microsoft.AspNetCore.Identity;

namespace Test.WebApi.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the TestWebApiUser class
    public class TestWebApiUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
