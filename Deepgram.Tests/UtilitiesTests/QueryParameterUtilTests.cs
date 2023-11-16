﻿namespace Deepgram.Tests.UtilitiesTests;

public class QueryParameterUtilTests
{
    [Test]
    public void GetParameters_Should_Return_String_When_Passing_String_Parameter()
    {
        //Arrange
        var prerecordedOptions = new AutoFaker<PrerecordedSchema>().Generate();
        var expected = HttpUtility.UrlEncode(prerecordedOptions.Model)!.ToLower();
        //Act
        var SUT = QueryParameterUtil.GetParameters(prerecordedOptions);

        //Assert
        Assert.That(SUT, Is.Not.Null);
        StringAssert.Contains($"{nameof(prerecordedOptions.Model).ToLower()}={expected}", SUT);
    }


    [Test]
    public void GetParameters_Should_Return_String_Respecting_Callback_Casing()
    {
        //Arrange
        var prerecordedOptions = new AutoFaker<PrerecordedSchema>().Generate();
        var CallBack = "https://Signed23.com";
        prerecordedOptions.Callback = CallBack;
        var expectedCallBack = HttpUtility.UrlEncode(CallBack);

        //Act
        var SUT = QueryParameterUtil.GetParameters(prerecordedOptions);

        //Assert
        Assert.That(SUT, Is.Not.Null);
        StringAssert.Contains($"{nameof(prerecordedOptions.Callback).ToLower()}={expectedCallBack}", SUT);
    }

    [Test]
    public void GetParameters_Should_Return_String_When_Passing_Int_Parameter()
    {
        //Arrange 
        var obj = new AutoFaker<PrerecordedSchema>().Generate();

        //Act
        var SUT = QueryParameterUtil.GetParameters(obj);

        //Assert
        Assert.That(SUT, Is.Not.Null);
        StringAssert.Contains($"alternatives={obj.Alternatives}", SUT);
    }

    [Test]
    public void GetParameters_Should_Return_String_When_Passing_Array_Parameter()
    {
        //Arrange

        var prerecordedOptions = new AutoFaker<PrerecordedSchema>().Generate();
        prerecordedOptions.Keywords = new string[] { "test" };

        //Act
        var SUT = QueryParameterUtil.GetParameters(prerecordedOptions);

        //Assert
        Assert.That(SUT, Is.Not.Null);
        StringAssert.Contains($"keywords={prerecordedOptions.Keywords[0].ToLower()}", SUT);

    }

    [Test]
    public void GetParameters_Should_Return_String_When_Passing_Decimal_Parameter()
    {

        //Arrange
        var prerecordedOptions = new AutoFaker<PrerecordedSchema>().Generate();
        prerecordedOptions.UtteranceSplit = 2.3;

        //Act
        // need to set manual as the precision can be very long and gets trimmed from autogenerated value

        var SUT = QueryParameterUtil.GetParameters(prerecordedOptions);

        //Assert
        Assert.That(SUT, Is.Not.Null);
        StringAssert.Contains($"utt_split={HttpUtility.UrlEncode(prerecordedOptions.UtteranceSplit.ToString())}", SUT);
    }

    [Test]
    public void GetParameters_Should_Return_String_When_Passing_Boolean_Parameter()
    {
        //Arrange 
        var obj = new AutoFaker<PrerecordedSchema>().Generate();

        //Act
        var SUT = QueryParameterUtil.GetParameters(obj);

        //Assert
        Assert.That(SUT, Is.Not.Null);
        StringAssert.Contains($"{nameof(obj.Paragraphs).ToLower()}={obj.Paragraphs.ToString()?.ToLower()}", SUT);
    }

    [Test]
    public void GetParameters_Should_Return_String_When_Passing_DateTime_Parameter()
    {
        //Arrange 
        var option = new AutoFaker<ExpirationOptions>().Generate();
        var obj = DateTime.Now;
        option.ExpirationDate = obj;
        var expected = ($"expiration_date={HttpUtility.UrlEncode(((DateTime)obj).ToString("yyyy-MM-dd"))}");


        //Act
        var SUT = QueryParameterUtil.GetParameters(option);

        //Assert
        Assert.That(SUT, Is.Not.Null);
        StringAssert.Contains(expected, SUT);
    }


    [Test]
    public void GetParameters_Should_Return_Empty_String_When_Parameter_Has_No_Values()
    {
        //Arrange 
        var obj = new PrerecordedSchema();

        //Act
        var SUT = QueryParameterUtil.GetParameters(obj);

        //Assert

        Assert.That(SUT, Is.Not.Null);
        Assert.That(SUT, Is.EqualTo(string.Empty));
    }
}