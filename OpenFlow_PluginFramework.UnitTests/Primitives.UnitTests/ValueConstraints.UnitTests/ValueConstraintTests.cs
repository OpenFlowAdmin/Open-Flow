using FluentAssertions;
using OpenFlow_PluginFramework.Primitives.ValueConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OpenFlow_PluginFramework.UnitTests.Primitives.UnitTests.ValueConstraints.UnitTests
{
    public class ValueConstraintTests
    {
        private ValueConstraint<int> _sut = new("Test Constraint", 5, (x) => x > 10 ? 10 : x);

        [Fact]
        public void AddToEndOfChain_ShouldCreateTotal_WhenChainEmpty()
        {
            List<ValueConstraint<int>> emptyChain = new();

            _sut.AddToEndOfChain(emptyChain);

            var result = _sut.TotalFunc(12);

            result.Should().Be(10);
        }

        [Fact]
        public void AddToEndOfChain_ShouldAddToEnd_WhenChainNotEmpty()
        {
            List<ValueConstraint<int>> nonEmptyChain = new() { new ValueConstraint<int>(null, null, (x) => 5) };

            _sut.AddToEndOfChain(nonEmptyChain);

            var result = _sut.TotalFunc(12);

            result.Should().Be(5);
        }
    }
}
