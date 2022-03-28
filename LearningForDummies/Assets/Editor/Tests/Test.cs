using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test
{
    // We only need to test string because the method cant get anything else
    [Test]
    [TestCase("Test")]
    [TestCase("5")]
    [TestCase("I am me")]
    [TestCase("%$§&")]
    [TestCase("")]
    public void loadRawReturnsNull(string catalogueName)
    {
        SaveSystem saveSystem = new SaveSystem();
        var answer = saveSystem.loadTextRawFromJson(catalogueName);
        Assert.AreEqual(answer, null);
    }
    [Test]
    [TestCase("Test")]
    [TestCase("5")]
    [TestCase("I am me")]
    [TestCase("%$§&")]
    [TestCase("")]
    public void loadPlayerProfileReturnsNull(string profileName)
    {
        SaveSystem saveSystem = new SaveSystem();
        var answer = saveSystem.loadPlayerProfileFromJson(profileName);
        Assert.AreEqual(answer, null);
    }
    [Test]
    public void loadCatalougesReturnsNull()
    {
        SaveSystem saveSystem = new SaveSystem();
        var answer = saveSystem.loadQuestionCataloguesFromJson();
        Assert.AreEqual(answer, null);
    }
    
}
