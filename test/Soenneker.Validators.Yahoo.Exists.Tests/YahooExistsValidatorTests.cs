using Soenneker.Tests.Attributes.Local;
using Soenneker.Validators.Yahoo.Exists.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading.Tasks;
using AwesomeAssertions;

namespace Soenneker.Validators.Yahoo.Exists.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class YahooExistsValidatorTests : HostedUnitTest
{
    private readonly IYahooExistsValidator _validator;

    public YahooExistsValidatorTests(Host host) : base(host)
    {
        _validator = Resolve<IYahooExistsValidator>(true);
    }

    [Test]
    public void Default()
    {
    }

    [Skip("Manual")]
    // [LocalOnly]
    public async Task Exists_should_be_true()
    {
        bool? result = await _validator.EmailExists("logan@yahoo.com");

        result.Should()
              .BeTrue();
    }
}
