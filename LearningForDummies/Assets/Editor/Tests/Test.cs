using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SaveData;

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
        //should be null for cataloguenames that are not in the app
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
        //should be null for all test values because the ral name is Player.pp
    }

    [Test]
    public void loadCatalougesReturnsList()
    {
        SaveSystem saveSystem = new SaveSystem();
        var answer = saveSystem.loadQuestionCataloguesFromJson();
        Assert.IsInstanceOf<List<QuestionCatalogue>>(answer);
        //should be of type List<QuestionCatalogues> if something is found in the foulder
    }

    [Test]
    [TestCase("Name", "Data")]
    public void saveRawReturnsOK(string name, string data)
    {
        SaveSystem saveSystem = new SaveSystem();
        SaveSystem.StatusCodes answer = saveSystem.saveRawJsonTextToJson(name, data);
        Assert.AreEqual(answer, SaveSystem.StatusCodes.OK);
        //should be ok
    }

    [Test]
    public void saveCatalogueReturnsOK()
    {
        QuestionCatalogue catalogue = new QuestionCatalogue();
        SaveSystem saveSystem = new SaveSystem();
        SaveSystem.StatusCodes answer = saveSystem.saveQuestionCatalogueToJson(catalogue);
        Assert.AreEqual(answer, SaveSystem.StatusCodes.OK);
        //should be ok
    }

    [Test]
    public void saveProfileReturnsOK()
    {
        PlayerProfile player = new PlayerProfile();
        SaveSystem saveSystem = new SaveSystem();
        SaveSystem.StatusCodes answer = saveSystem.savePlayerProfileToJson(player);
        Assert.AreEqual(answer, SaveSystem.StatusCodes.OK);
        //should be ok
    }
}
