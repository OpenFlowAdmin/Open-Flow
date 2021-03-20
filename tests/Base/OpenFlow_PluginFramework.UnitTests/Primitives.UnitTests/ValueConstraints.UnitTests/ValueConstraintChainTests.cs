using FluentAssertions;
using OpenFlow_PluginFramework.Primitives.ValueConstraints;
using Xunit;

namespace OpenFlow_PluginFramework.UnitTests.Primitives.UnitTests.ValueConstraints.UnitTests
{
    public class ValueConstraintChainTests
    {
        private ValueConstraintChain<int> _sut = new();

        [Fact]
        public void AddConstraint_ShouldChainConstraint_WhenChainEmpty()
        {
            const string ConstraintName = "Test Constraint";
            const int ConstraintValue = 5;

            _sut.AddConstraint(new ValueConstraint<int>(ConstraintName, ConstraintValue, (x) => ConstraintValue));

            var result = _sut.TotalConstraint(10);

            result.Should().Be(ConstraintValue);
            _sut.ConstraintValues.Should().Contain(ConstraintName, ConstraintValue);
            _sut.ConstraintValues.Count.Should().Be(1);
        }

        [Fact]
        public void AddConstraint_ShouldChainDelegate_WhenChainEmpty()
        {
            const int ConstraintValue = 5;
            _sut.AddConstraint((x) => ConstraintValue);

            var result = _sut.TotalConstraint(10);

            result.Should().Be(5);
            _sut.ConstraintValues.Should().BeEmpty();
        }

        [Fact]
        public void AddConstraint_ShouldChainConstraint_WhenChainNotEmpty()
        {
            const string ConstraintName = "Test Constraint";
            const int ConstraintValue = 5;

            _sut.AddConstraint((x) => ConstraintValue);
            _sut.AddConstraint(new ValueConstraint<int>(ConstraintName, ConstraintValue, (x) => x < 10 ? x : 10));

            var result = _sut.TotalConstraint(15);

            result.Should().Be(ConstraintValue);
            _sut.ConstraintValues.Should().Contain(ConstraintName, ConstraintValue);
            _sut.ConstraintValues.Count.Should().Be(1);
        }

        [Fact]
        public void AddConstraint_ShouldChainDelegate_WhenChainNotEmpty()
        {
            const int ConstraintValue = 5;

            _sut.AddConstraint((x) => ConstraintValue);
            _sut.AddConstraint((x) => x < 10 ? x : 10);

            var result = _sut.TotalConstraint(15);

            result.Should().Be(5);
            _sut.ConstraintValues.Should().BeEmpty();
        }
    }
}
