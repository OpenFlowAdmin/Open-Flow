using FluentAssertions;
using OpenFlow_PluginFramework.Primitives;
using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using OpenFlow_PluginFramework.Primitives.ValueConstraints;
using Xunit;

namespace OpenFlow_PluginFramework.UnitTests.Primitives.UnitTests.TypeDefinition.UnitTests
{
    public class TypeDefinitionTests
    {
        private readonly TypeDefinition<double> _sut;

        public TypeDefinitionTests()
        {
            _sut = new TypeDefinition<double>();
        }

        [Fact]
        public void TrySetValue_ShouldPass_WhenInputValid()
        {
            var result = _sut.TryConstraintValue(10.0, out _);

            result.Should().BeTrue();
        }

        [Fact]
        public void TrySetValue_ShouldFail_OnTypeDifference()
        {
            var result = _sut.TryConstraintValue("Test", out _);

            result.Should().BeFalse();
        }

        [Fact]
        public void WithConstraint_ShouldConstrain_WithLambda()
        {
            _sut.WithConstraint((x) => 5.0);

            var setValueBool = _sut.TryConstraintValue(10.0, out object result);

            setValueBool.Should().BeTrue();
            result.Should().BeEquivalentTo(5.0);
            _sut.ConstraintValues.Should().BeEmpty();
        }

        [Fact]
        public void WithConstraint_ShouldConstrain_WithConstraint()
        {
            const string constraintName = "Test Constraint";
            const double constraintValue = 5.0;

            _sut.WithConstraint(new ValueConstraint<double>(constraintName, constraintValue, (x) => 5.0));

            var setValueBool = _sut.TryConstraintValue(10.0, out object result);

            setValueBool.Should().BeTrue();
            result.Should().BeEquivalentTo(5.0);
            _sut.ConstraintValues.Count.Should().Be(1);
            _sut.ConstraintValues.Should().Contain(constraintName, constraintValue);
        }
    }
}
