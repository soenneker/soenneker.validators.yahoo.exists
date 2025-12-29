using Soenneker.Facts.Local;
using Soenneker.Validators.Yahoo.Exists.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;
using System.Threading.Tasks;
using AwesomeAssertions;
using Soenneker.Facts.Manual;

namespace Soenneker.Validators.Yahoo.Exists.Tests;

[Collection("Collection")]
public class YahooExistsValidatorTests : FixturedUnitTest
{
    private readonly IYahooExistsValidator _validator;

    public YahooExistsValidatorTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _validator = Resolve<IYahooExistsValidator>(true);
    }

    [Fact]
    public void Default()
    {
    }

    [ManualFact]
    // [LocalFact]
    public async Task Exists_should_be_true()
    {
        bool? result = await _validator.EmailExists("logan@yahoo.com");

        result.Should()
              .BeTrue();
    }
}