using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test
{
    // A Test behaves as an ordinary method
    [Test]
    [TestCase("Test")]
    public void loadReturnsNull(string catalogueName)
    {
        SaveSystem saveSystem = new SaveSystem();
        var answer = saveSystem.loadTextRawFromJson(catalogueName);
        Assert.AreEqual(answer, null);
    }
    
}
