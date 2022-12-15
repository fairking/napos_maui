using Napos.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Napos.Core.Tests.All.Helpers
{
    public class EncryptHelperTests
    {
        public EncryptHelperTests()
        {

        }

        [Fact]
        public async Task MixByteArrays()
        {
            // First is smaller than secod
            {
                var one = "Hello";
                var two = "World!!!";

                var result = Encoding.UTF8.GetString(EncryptHelper.MixByteArrays(Encoding.UTF8.GetBytes(one), Encoding.UTF8.GetBytes(two)));

                Assert.Equal("HWeolrllod!!!", result);
            }

            // Second is smaller than first
            {
                var one = "Hello";
                var two = "Bee";

                var result = Encoding.UTF8.GetString(EncryptHelper.MixByteArrays(Encoding.UTF8.GetBytes(one), Encoding.UTF8.GetBytes(two)));

                Assert.Equal("HBeelelo", result);
            }

            // First is empty
            {
                var one = "";
                var two = "World!";

                var result = Encoding.UTF8.GetString(EncryptHelper.MixByteArrays(Encoding.UTF8.GetBytes(one), Encoding.UTF8.GetBytes(two)));

                Assert.Equal("World!", result);
            }

            // Second is empty
            {
                var one = "Hello";
                var two = "";

                var result = Encoding.UTF8.GetString(EncryptHelper.MixByteArrays(Encoding.UTF8.GetBytes(one), Encoding.UTF8.GetBytes(two)));

                Assert.Equal("Hello", result);
            }
        }

        [Fact]
        public async Task CreatePasswordHash()
        {
            // Test Hash with Salt
            {
                var salt = "123";
                var hash1 = EncryptHelper.CreateHash("HelloWorld!", salt);
                var hash2 = EncryptHelper.CreateHash("HelloWorld!", salt);
                Assert.NotEmpty(hash1);
                Assert.Equal(88, hash1.Length);
                Assert.Equal(hash2, hash1);
            }

            // Test Hash without Salt
            {
                var hash1 = EncryptHelper.CreateHash("HelloWorld! HelloWorld!");
                var hash2 = EncryptHelper.CreateHash("HelloWorld! HelloWorld!");
                Assert.NotEmpty(hash1);
                Assert.Equal(88, hash1.Length);
                Assert.Equal(hash2, hash1);
            }

            // Test hashes are not equal with Salt
            {
                var hash1 = EncryptHelper.CreateHash("a", "c");
                var hash2 = EncryptHelper.CreateHash("b", "c");
                Assert.NotEmpty(hash1);
                Assert.NotEmpty(hash2);
                Assert.Equal(88, hash1.Length);
                Assert.Equal(88, hash2.Length);
                Assert.NotEqual(hash2, hash1);
            }

            // Test hashes are not equal without Salt
            {
                var hash1 = EncryptHelper.CreateHash("a");
                var hash2 = EncryptHelper.CreateHash("b");
                Assert.NotEmpty(hash1);
                Assert.NotEmpty(hash2);
                Assert.Equal(88, hash1.Length);
                Assert.Equal(88, hash2.Length);
                Assert.NotEqual(hash2, hash1);
            }
        }
    }
}
