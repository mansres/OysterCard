using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OysterCardSystem.Core;

namespace OysterCardSystem.Tests.CoreTest
{
    /// <summary>
    /// Test case for Smart card
    /// </summary>
    [TestClass]
    public class SmartCardTest
    {
        [TestMethod]
        [ExpectedException(typeof(FareException), "You don't have enough balance!")]
        public void TestValidateExeption()
        {
            SmartCard card = new SmartCard(30f);
            card.Validate(40);
        }

        [TestMethod]
        [ExpectedException(typeof(FareException), "You don't have enough balance!")]
        public void TestOutException()
        {
            SmartCard card = new SmartCard(30f);
            card.Out(40);
        }
    }
}
