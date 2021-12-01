using AutoFixture.Xunit2;

namespace HealthChecker.Tests.AutoFixture
{
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects)
            : base(new AutoMoqDataAttribute(), objects) { }
    }
}
