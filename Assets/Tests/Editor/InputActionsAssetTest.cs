using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Linq;


/// <summary> 
/// Sanitycheck that every action our code cares about
/// actually exists in the generated Input Actions asset
/// </summary>
public class InputActionsAssetTest 
{
    // 1.) Instantiate the generated Input Actions asset

    readonly InputActions inputActions = new InputActions();

    // 2.) List out all the action names our code will expect
    // keep this in sync with .inputactions file
    // and with any scriptableobject references

    readonly string[] expectedActionNames = new string[]
    {
        "Move",
        "Look",
        "Jump",
        "Crouch",
        "Sprint"
    };

    [Test]
    public void EveryExpectedAction_IsPresentInAsset()
    {
        //3.) grab the actual asset instance
        // InputActions.asset is the asset under the hood

        var asset = inputActions.asset;

        //4.) loop through each expected name

        foreach (var name in expectedActionNames)
        {
            var action = asset.FindAction(name, throwIfNotFound: false);

            //5.) Asset it's not null, if it is null, the test will fail and tell us which one

            Assert.NotNull(action, $"Action '{name}' not found in InputActions asset, did you rename or delite it?");
        }
    }

}
