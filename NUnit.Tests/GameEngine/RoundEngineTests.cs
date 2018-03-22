﻿using NUnit.Framework;

using Game.GameEngine;
using Game.Models;
using Game.ViewModels;
using Game.Services;

using NUnit.Tests.Models.Default;
using Xamarin.Forms.Mocks;

namespace NUnit.Tests.GameEngine
{
    [TestFixture]
    public class RoundEngineTests
    {
        #region RoundBasics
        [Test]
        public void RoundEngine_Instantiate_Should_Pass()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var Actual = new RoundEngine();
            Assert.AreNotEqual(null, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_StartRound_Should_Pass()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            var Actual = myRoundEngine.BattleScore.RoundCount;
            var Expected = 1;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }


        [Test]
        public void RoundEngine_EndRound_Should_Pass()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();
            myRoundEngine.EndRound();

            var Actual = myRoundEngine.BattleScore.RoundCount;
            var Expected = 1; // Started out as zero, nothing happened...

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
        #endregion RoundBasics

        #region RoundNextTurn

        [Test]
        public void RoundEngine_RoundNextTurn_No_Characters_Should_Return_GameOver()
        {
            MockForms.Init();

            // No characters, so return should be game over...

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            var Actual = myRoundEngine.RoundNextTurn();
            var Expected = RoundEnum.GameOver;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_RoundNextTurn_2_Characters_Should_Return_NextTurn()
        {
            MockForms.Init();

            // No characters, so return should be game over...

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();

            // Add 2 monsters
            myRoundEngine.MonsterList.Add(new Monster(DefaultModels.MonsterDefault()));
            myRoundEngine.MonsterList.Add(new Monster(DefaultModels.MonsterDefault()));

            // Add 2 characters
            myRoundEngine.CharacterList.Add(new Character(DefaultModels.CharacterDefault()));
            myRoundEngine.CharacterList.Add(new Character(DefaultModels.CharacterDefault()));

            // Start
            myRoundEngine.StartRound();

            // Do the turn...
            var Actual = myRoundEngine.RoundNextTurn();

            // There are 2 characters, and 2 monsters, so the first turn should happen, and it is now next turn...

            var Expected = RoundEnum.NextTurn;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_RoundNextTurn_Characters_MonstersDead_Should_Return_NewRound()
        {
            MockForms.Init();

            // No characters, so return should be game over...

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            // Start
            myRoundEngine.StartRound();
            // Clear the monster list

            // Add weak monsters
            var myMonsterWeak = new Monster(DefaultModels.MonsterDefault());
            myMonsterWeak.ScaleLevel(1);

            myRoundEngine.MonsterList.Clear();  // start fresh, because it was loaded already with 6...
            myRoundEngine.MonsterList.Add(myMonsterWeak);

            // Add strong character
            var myCharacterStrong = new Character(DefaultModels.CharacterDefault());
            myCharacterStrong.ScaleLevel(20);
            myRoundEngine.CharacterList.Add(myCharacterStrong);

            // Character, should kill the monster in the first round.
            // So the check for the second round will say Round over...
            var FirstRound = myRoundEngine.RoundNextTurn();
            var Actual = myRoundEngine.RoundNextTurn();

            var Expected = RoundEnum.NewRound;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(RoundEnum.NextTurn, FirstRound, TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_RoundNextTurn_2Characters_1Monster_Should_Take_2_Rounds()
        {
            MockForms.Init();

            // No characters, so return should be game over...

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();

            // Start
            myRoundEngine.StartRound();

            // Add moderate monsters
            // First monster
            var myMonsterWeak = new Monster(DefaultModels.MonsterDefault());
            myMonsterWeak.ScaleLevel(2);
            myMonsterWeak.Attribute.CurrentHealth = 7; // need to set to enough to last 2 rounds...

            myRoundEngine.MonsterList.Clear();  // start fresh, because it was loaded already with 6...
            myRoundEngine.MonsterList.Add(myMonsterWeak);

            // Add weak character for first...
            var myCharacterStrong = new Character(DefaultModels.CharacterDefault());
            myCharacterStrong.ScaleLevel(10);
            myRoundEngine.CharacterList.Add(myCharacterStrong);

            // Add strong character for second
            myCharacterStrong = new Character(DefaultModels.CharacterDefault());
            myCharacterStrong.ScaleLevel(20);
            myRoundEngine.CharacterList.Add(myCharacterStrong);

            // Should be Character 20, Character 10, Monster 5

            // Force rolls to 18 for to hit...
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 18);

            // Character, should kill the monster in the first round.
            // So the check for the second round will say Round over...
            var FirstRound = myRoundEngine.RoundNextTurn();     // Character 20 Goes
            var SecondRound = myRoundEngine.RoundNextTurn();    // Character 10 Goes
            var Actual = myRoundEngine.RoundNextTurn();         // Over...

            // Reset
            GameGlobals.ToggleRandomState();

            var Expected = RoundEnum.NewRound;

            Assert.AreEqual(Expected, Actual, "Status " + TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(2, myRoundEngine.BattleScore.TurnCount, "TurnCount " + TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(1, myRoundEngine.BattleScore.RoundCount, "RoundCount " + TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_RoundNextTurn_2Characters_1Monster_Should_Take_3_Rounds()
        {
            MockForms.Init();

            // No characters, so return should be game over...

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            // Start
            myRoundEngine.StartRound();

            // Add moderate monsters
            // First monster
            var myMonsterWeak = new Monster(DefaultModels.MonsterDefault());
            myMonsterWeak.ScaleLevel(2);
            myMonsterWeak.Attribute.CurrentHealth = 10; // need to set to enough to last 2 rounds...

            myRoundEngine.MonsterList.Clear();  // start fresh, because it was loaded already with 6...
            myRoundEngine.MonsterList.Add(myMonsterWeak);

            // Add weak character for first...
            var myCharacterStrong = new Character(DefaultModels.CharacterDefault());
            myCharacterStrong.ScaleLevel(10);
            myRoundEngine.CharacterList.Add(myCharacterStrong);

            // Add strong character for second
            myCharacterStrong = new Character(DefaultModels.CharacterDefault());
            myCharacterStrong.ScaleLevel(20);
            myRoundEngine.CharacterList.Add(myCharacterStrong);

            // Should be Character 20, Character 10, Monster 5

            // Force rolls to 18 for to hit...
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 18);

            // Character, should kill the monster in the first round.
            // So the check for the second round will say Round over...
            var FirstRound = myRoundEngine.RoundNextTurn();     // Character 20 
            var SecondRound = myRoundEngine.RoundNextTurn();    // Character 10 goes
            var ThirdRound = myRoundEngine.RoundNextTurn();     // Monster goes
            var FourthRound = myRoundEngine.RoundNextTurn();    // Character 20 goes, kills monster...
            var Actual = myRoundEngine.RoundNextTurn();         // over...

            // Reset
            GameGlobals.ToggleRandomState();

            var Expected = RoundEnum.NewRound;

            Assert.AreEqual(Expected, Actual, "Status " + TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(4, myRoundEngine.BattleScore.TurnCount, "TurnCount " + TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(1, myRoundEngine.BattleScore.RoundCount, "RoundCount " + TestContext.CurrentContext.Test.Name);
        }

        #endregion RoundNextTurn

        #region GetNextPlayerInList


        [Test]
        public void RoundEngine_GetNextPlayerInList_Null_PlayerList_Should_Skip()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            myRoundEngine.PlayerCurrent = null;
            myRoundEngine.PlayerList.Clear();

            var Actual = myRoundEngine.GetNextPlayerInList();
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_GetNextPlayerInList_Null_PlayerList_CurrentPlayer_Valid_Should_Skip()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            myRoundEngine.PlayerCurrent = new PlayerInfo();
            myRoundEngine.PlayerList.Clear();

            var Actual = myRoundEngine.GetNextPlayerInList();
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_GetNextPlayerInList_1Player_CurrentPlayer_Null_Should_ReturnFirst()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            myRoundEngine.PlayerList.Clear();

            var myFirst = new PlayerInfo();
            myRoundEngine.PlayerList.Add(myFirst);

            myRoundEngine.PlayerCurrent = null;

            var Actual = myRoundEngine.GetNextPlayerInList();
            var Expected = myFirst;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_GetNextPlayerInList_3Player_CurrentPlayer_1_Should_Return_2nd()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            myRoundEngine.PlayerList.Clear();

            var myFirst = new PlayerInfo();
            myRoundEngine.PlayerList.Add(myFirst);

            var mySecond = new PlayerInfo();
            myRoundEngine.PlayerList.Add(mySecond);

            var myThird = new PlayerInfo();
            myRoundEngine.PlayerList.Add(myThird);

            myRoundEngine.PlayerCurrent = myFirst;

            var Actual = myRoundEngine.GetNextPlayerInList();
            var Expected = mySecond;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void RoundEngine_GetNextPlayerInList_3Player_CurrentPlayer_3_Should_Return_1st()
        {
            MockForms.Init();

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();

            myRoundEngine.PlayerList.Clear();

            var myFirst = new PlayerInfo();
            myRoundEngine.PlayerList.Add(myFirst);

            var mySecond = new PlayerInfo();
            myRoundEngine.PlayerList.Add(mySecond);

            var myThird = new PlayerInfo();
            myRoundEngine.PlayerList.Add(myThird);

            myRoundEngine.PlayerCurrent = myThird;

            var Actual = myRoundEngine.GetNextPlayerInList();
            var Expected = myFirst;

            Assert.AreEqual(Expected.Guid, Actual.Guid, TestContext.CurrentContext.Test.Name);
        }
        #endregion GetNextPlayerInList

        [Test]
        public void RoundEngine_StartRound_Zero_Monsters_Should_Create_6()
        {
            MockForms.Init();

            // Clear the datastore...
            var myMonsterViewModel = MonstersViewModel.Instance;
            var canExecute = myMonsterViewModel.LoadDataCommand.CanExecute(null);
            myMonsterViewModel.LoadDataCommand.Execute(null);
            myMonsterViewModel.Dataset.Add(new Monster());
            var ListCount = myMonsterViewModel.Dataset.Count;
            myMonsterViewModel.Dataset.Clear();
            MockDataStore.Instance.DeleteTables();  // Remove the data from Mock as well...

            // Can create a new Round engine...
            var myRoundEngine = new RoundEngine();
            myRoundEngine.StartRound();


            var Actual = myRoundEngine.MonsterList.Count;
            var Expected = 6;

            // Restore the datastore
            MockDataStore.Instance.InitializeDatabaseNewTables();

            Assert.AreEqual(Expected, Actual, "Monster List" + TestContext.CurrentContext.Test.Name);
            Assert.AreNotEqual(Expected, ListCount, "View Model " + TestContext.CurrentContext.Test.Name);

        }

    }
}

