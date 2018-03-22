﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.GameEngine;
using Game.Models;
using Game.ViewModels;

using NUnit.Tests.Models.Default;
using Xamarin.Forms.Mocks;
using Game.Services;

namespace NUnit.Tests.GameEngine
{
    [TestFixture]
    public class TurnEngineTests
    {
        #region TurnAsAttack
        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_Null_DefenderList_Should_Skip()
        {
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();

            var myTurnEngine = new TurnEngine();

            var Status = myTurnEngine.TurnAsAttack(Attacker,1, null,1);
            var Actual = Status;
            bool Expected = false;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_Null_DefenderList_Should_Skip()
        {
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.MonsterDefault();

            var myTurnEngine = new TurnEngine();

            var Status = myTurnEngine.TurnAsAttack(Attacker, 1, null, 1);
            var Actual = Status;
            bool Expected = false;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }


        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_Null_Should_Skip()
        {
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var myDefaultMonster = new Character();

            var myTurnEngine = new TurnEngine();

            var Status = myTurnEngine.TurnAsAttack((Monster)null, 1, myDefaultMonster, 1);
            var Actual = Status;
            bool Expected = false;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_Null_Should_Skip()
        {
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            var myDefaultMonster = new Monster();

            var myTurnEngine = new TurnEngine();

            var Status = myTurnEngine.TurnAsAttack(null,1, myDefaultMonster,1);
            var Actual = Status;
            bool Expected = false;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_DefenderList_CriticalMiss_Should_Pass()
        {
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            Attacker.Name = "Fighter";

            var myDefaultMonster = DefaultModels.MonsterDefault();
            myDefaultMonster.Name = "Rat";

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            GameGlobals.ForceToHitValue = 1; // Force a miss

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultMonster.GetDefense() + myDefaultMonster.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultMonster, DefenseScore);
            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }


        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_DefenderList_CriticalMiss_Should_Pass()
        {
            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.MonsterDefault();
            Attacker.Name = "Rat";

            var myDefaultCharacter = DefaultModels.CharacterDefault();
            myDefaultCharacter.Name = "Fighter";

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            GameGlobals.ForceToHitValue = 1; // Force a miss

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultCharacter.GetDefense() + myDefaultCharacter.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultCharacter, DefenseScore);
            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_DefenderList_CriticalHit_Should_Pass()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            Attacker.Name = "Fighter";

            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(1);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            // Force a Critical Hit
            GameGlobals.ForceToHitValue = 20;

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultMonster.GetDefense() + myDefaultMonster.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultMonster, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;
            var LifeStatus = myDefaultMonster.Alive;
            
            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_DefenderList_CriticalHit_Should_Pass()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.MonsterDefault();
            Attacker.Name = "Rat";

            var myDefaultCharacter = new Character(DefaultModels.CharacterDefault());
            myDefaultCharacter.Name = "Fighter";
            myDefaultCharacter.ScaleLevel(1);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            GameGlobals.ForceToHitValue = 20; // Force a hit

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultCharacter.GetDefense() + myDefaultCharacter.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultCharacter, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;
            var LifeStatus = myDefaultCharacter.Alive;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_Defender_CriticalHit_PowerfullHit_Should_Kill()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            Attacker.Name = "Fighter";
            Attacker.ScaleLevel(20);

            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(1);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            GameGlobals.ForceToHitValue = 20; // Force a hit

            // Should Kill because level 20 hit on level 1 monster for Critical is more damage than health...

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultMonster.GetDefense() + myDefaultMonster.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultMonster, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;
            var LifeStatus = myDefaultMonster.Alive;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(false, myDefaultMonster.Alive, TestContext.CurrentContext.Test.Name);
        }


        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_Defender_CriticalHit_PowerfullHit_Should_Kill()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.MonsterDefault();
            Attacker.Name = "Rat";
            Attacker.ScaleLevel(20);

            var myDefaultCharacter = new Character(DefaultModels.CharacterDefault());
            myDefaultCharacter.Name = "Fighter";
            myDefaultCharacter.ScaleLevel(1);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            GameGlobals.ForceToHitValue = 20; // Force a hit

            // Should Kill because level 20 hit on level 1 Character for Critical is more damage than health...

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultCharacter.GetDefense() + myDefaultCharacter.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultCharacter, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;
            var LifeStatus = myDefaultCharacter.Alive;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(false, myDefaultCharacter.Alive, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_Defender_Die_With_Item_Should_Drop()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            Attacker.Name = "Fighter";
            Attacker.ScaleLevel(20);

            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(1);

            // Add Uniqueitem
            var myItem = new Item
            {
                Attribute = AttributeEnum.Attack,
                Location = ItemLocationEnum.Feet,
                Value = 1
            };
            ItemsViewModel.Instance.AddAsync(myItem).GetAwaiter().GetResult();  // Register Item to DataSet
            myDefaultMonster.UniqueItem = myItem.Guid;

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            // Get Score, and remember item.
            var BeforeItemDropList = myTurnEngine.BattleScore.ItemsDroppedList;

            GameGlobals.ForceToHitValue = 20; // Force a hit

            // Should Kill because level 20 hit on level 1 monster for Critical is more damage than health...
            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultMonster.GetDefense() + myDefaultMonster.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultMonster, DefenseScore);


            // Item should drop...

            // Reset
            GameGlobals.ToggleRandomState();

            // Need to get Score
            // See if Item is now in the score list...
            var AfterItemDropList = myTurnEngine.BattleScore.ItemsDroppedList;
            Assert.AreNotEqual(BeforeItemDropList, AfterItemDropList, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_Defender_Die_With_Item_Should_Drop()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.MonsterDefault();
            Attacker.Name = "Rat";
            Attacker.ScaleLevel(20);

            var myDefaultCharacter = new Character(DefaultModels.CharacterDefault());
            myDefaultCharacter.Name = "Fighter";
            myDefaultCharacter.ScaleLevel(1);

            // Add Uniqueitem
            var myItem = new Item
            {
                Attribute = AttributeEnum.Attack,
                Location = ItemLocationEnum.Feet,
                Value = 1
            };
            ItemsViewModel.Instance.AddAsync(myItem).GetAwaiter().GetResult();  // Register Item to DataSet
            myDefaultCharacter.PrimaryHand = myItem.Guid;

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            // Get Score, and remember item.
            var BeforeItemDropList = myTurnEngine.BattleScore.ItemsDroppedList;

            GameGlobals.ForceToHitValue = 20; // Force a hit

            // Should Kill because level 20 hit on level 1 Character for Critical is more damage than health...
            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultCharacter.GetDefense() + myDefaultCharacter.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultCharacter, DefenseScore);


            // Item should drop...

            // Reset
            GameGlobals.ToggleRandomState();

            // Need to get Score
            // See if Item is now in the score list...
            var AfterItemDropList = myTurnEngine.BattleScore.ItemsDroppedList;
            Assert.AreNotEqual(BeforeItemDropList, AfterItemDropList, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_DefenderList_Hit_Should_Pass()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            Attacker.Name = "Fighter";

            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(2);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            GameGlobals.ForceToHitValue = 19; // Force a miss

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultMonster.GetDefense() + myDefaultMonster.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultMonster, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_DefenderList_Hit_Should_Pass()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.MonsterDefault();
            Attacker.Name = "Rat";

            var myDefaultCharacter = new Character(DefaultModels.CharacterDefault());
            myDefaultCharacter.Name = "Fighter";
            myDefaultCharacter.ScaleLevel(2);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            GameGlobals.ForceToHitValue = 19; // Force a miss

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultCharacter.GetDefense() + myDefaultCharacter.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultCharacter, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_TurnAsAttack_Character_Attack_DefenderList_Miss_Should_Pass()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            Attacker.Name = "Fighter";

            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(20);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            GameGlobals.ForceToHitValue = 2; // Force a miss

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultMonster.GetDefense() + myDefaultMonster.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultMonster, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
        
        [Test]
        public void TurnEngine_TurnAsAttack_Monster_Attack_DefenderList_Miss_Should_Pass()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.MonsterDefault();
            Attacker.Name = "Rat";

            var myDefaultCharacter = new Character(DefaultModels.CharacterDefault());
            myDefaultCharacter.Name = "Fighter";
            myDefaultCharacter.ScaleLevel(20);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            GameGlobals.ForceToHitValue = 2; // Force a miss

            var AttackScore = Attacker.Level + Attacker.GetAttack();
            var DefenseScore = myDefaultCharacter.GetDefense() + myDefaultCharacter.Level;

            var Status = myTurnEngine.TurnAsAttack(Attacker, AttackScore, myDefaultCharacter, DefenseScore);

            var Actual = Status;
            bool Expected = true;

            var ActualString = myTurnEngine.TurnMessage;
            var ActualResult = myTurnEngine.HitStatus;
            var ActualDamage = myTurnEngine.DamageAmount;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
        #endregion TurnAsAttack

        #region AttackChoice
        [Test]
        public void TurnEngine_Turn_AttackChoice_Character_Null_DefenderList_Should_Skip()
        {
            var Attacker = DefaultModels.CharacterDefault();

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList = null;

            var Actual = myTurnEngine.AttackChoice(Attacker);
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_Turn_AttackChoice_Character_Valid_Attacker_Empty_Defender_Should_Pass()
        {
            var Attacker = DefaultModels.CharacterDefault();

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList = new List<Monster>();

            var Actual = myTurnEngine.AttackChoice(Attacker);
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_Turn_AttackChoice_Character_Valid_Attacker_Valid_Defender_Should_Pass()
        {
            var Attacker = DefaultModels.CharacterDefault();

            var DefenderList = new List<Monster>();
            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(20);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            var Actual = myTurnEngine.AttackChoice(Attacker);
            var Expected = myDefaultMonster;

            Assert.AreEqual(Expected.Guid, Actual.Guid, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_Turn_AttackChoice_Character_Valid_Attacker_Valid_Defender_Alive_False_Should_Skip()
        {
            var Attacker = DefaultModels.CharacterDefault();

            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(20);
            myDefaultMonster.Alive = false; //dead...

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            var Actual = myTurnEngine.AttackChoice(Attacker);
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_Turn_AttackChoice_Monster_Null_DefenderList_Should_Skip()
        {
            var Attacker = DefaultModels.MonsterDefault();

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList = null;

            var Actual = myTurnEngine.AttackChoice(Attacker);
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_Turn_AttackChoice_Monster_Valid_Attacker_Empty_Defender_Should_Pass()
        {
            var Attacker = DefaultModels.MonsterDefault();

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList = new List<Character>();

            var Actual = myTurnEngine.AttackChoice(Attacker);
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_Turn_AttackChoice_Monster_Valid_Attacker_Valid_Defender_Should_Pass()
        {
            var Attacker = DefaultModels.MonsterDefault();

            var DefenderList = new List<Character>();
            var myDefaultCharacter = new Character(DefaultModels.CharacterDefault());
            myDefaultCharacter.Name = "Fighter";
            myDefaultCharacter.ScaleLevel(20);

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            var Actual = myTurnEngine.AttackChoice(Attacker);
            var Expected = myDefaultCharacter;

            Assert.AreEqual(Expected.Guid, Actual.Guid, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_Turn_AttackChoice_Monster_Valid_Attacker_Valid_Defender_Alive_False_Should_Skip()
        {
            var Attacker = DefaultModels.MonsterDefault();

            var myDefaultCharacter = new Character(DefaultModels.CharacterDefault());
            myDefaultCharacter.Name = "Fighter";
            myDefaultCharacter.ScaleLevel(20);
            myDefaultCharacter.Alive = false; //dead...

            var myTurnEngine = new TurnEngine();
            myTurnEngine.CharacterList.Add(myDefaultCharacter);

            var Actual = myTurnEngine.AttackChoice(Attacker);
            object Expected = null;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        #endregion AttackChoice

        #region TakeTurn

        [Test]
        public void TurnEngine_TakeTurn_Attack_Valid_Defender_Valid_Defender_Should_Die()
        {
            MockForms.Init();

            // Turn off random numbers
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var Attacker = DefaultModels.CharacterDefault();
            Attacker.Name = "Fighter";
            Attacker.ScaleLevel(20);

            var myDefaultMonster = new Monster(DefaultModels.MonsterDefault());
            myDefaultMonster.Name = "Rat";
            myDefaultMonster.ScaleLevel(1);

            // Add Uniqueitem
            var myItem = new Item
            {
                Attribute = AttributeEnum.Attack,
                Location = ItemLocationEnum.Feet,
                Value = 1
            };
            ItemsViewModel.Instance.AddAsync(myItem).GetAwaiter().GetResult();  // Register Item to DataSet
            myDefaultMonster.UniqueItem = myItem.Guid;

            var myTurnEngine = new TurnEngine();
            myTurnEngine.MonsterList.Add(myDefaultMonster);

            // Get Score, and remember item.
            var BeforeItemDropList = myTurnEngine.BattleScore.ItemsDroppedList;

            GameGlobals.ForceToHitValue = 20; // Force a hit

            var BeforeMonsterList = myTurnEngine.MonsterList.Count();

            // Instead of calling TurnAsAttack, call TakeTurn...
            var Status = myTurnEngine.TakeTurn(Attacker);

            var AfterMonsterList = myTurnEngine.MonsterList.Count();

            // Item should drop...
            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(BeforeMonsterList-1, AfterMonsterList, TestContext.CurrentContext.Test.Name);
        }
        #endregion TakeTurn

        #region RollToHitTarget
        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Higher_Than_Defender_Roll_1_Should_CriticalMiss()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 1);

            var myTurnEngine = new TurnEngine();

            var Attacker = 100;
            var Defender = 1;

            var Actual = myTurnEngine.RollToHitTarget(Attacker,Defender);
            var Expected = HitStatusEnum.CriticalMiss;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Higher_Than_Defender_Roll_2_Should_Hit()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 2);

            var myTurnEngine = new TurnEngine();

            var Attacker = 100;
            var Defender = 1;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.Hit;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Higher_Than_Defender_Roll_19_Should_Hit()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 19);

            var myTurnEngine = new TurnEngine();

            var Attacker = 10;
            var Defender = 1;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.Hit;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Higher_Than_Defender_Roll_20_Should_CriticalHit()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var myTurnEngine = new TurnEngine();

            var Attacker = 10;
            var Defender = 1;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.CriticalHit;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Higher_Than_Defender_Roll_Neg1_Should_Miss()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, -1);

            var myTurnEngine = new TurnEngine();

            var Attacker = 100;
            var Defender = 1;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.CriticalMiss;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Lower_Than_Defender_Roll_Neg1_Should_Miss()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, -1);

            var myTurnEngine = new TurnEngine();

            var Attacker = 1;
            var Defender = 10;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.CriticalMiss;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Lower_Than_Defender_Roll_1_Should_CriticalMiss()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 1);

            var myTurnEngine = new TurnEngine();

            var Attacker = 1;
            var Defender = 10;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.CriticalMiss;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Lower_Than_Defender_Roll_19_Should_Miss()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 19);

            var myTurnEngine = new TurnEngine();

            var Attacker = 1;
            var Defender = 100;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.Miss;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_RollToHitTarget_Attacker_Lower_Than_Defender_Roll_20_Should_CriticalHit()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 20);

            var myTurnEngine = new TurnEngine();

            var Attacker = 1;
            var Defender = 10;

            var Actual = myTurnEngine.RollToHitTarget(Attacker, Defender);
            var Expected = HitStatusEnum.CriticalHit;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        #endregion RollToHitTarget

        #region GetRandomMonsterItemDrops
        [Test]
        public void TurnEngine_GetRandomMonsterItemDrops_1_Roll_Should_Return_List()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 1);
            GameGlobals.ForcedRandomValue = 1;

            var myTurnEngine = new TurnEngine();

            var roundCount = 1;

            var Actual = myTurnEngine.GetRandomMonsterItemDrops(roundCount);

            // Should roll for 1 item, and return it...
            var Expected = 1;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual.Count(), TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_GetRandomMonsterItemDrops_3_Roll_Should_Return_List()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 1);
            GameGlobals.ForcedRandomValue = 3;

            var myTurnEngine = new TurnEngine();

            var roundCount = 1;

            var Actual = myTurnEngine.GetRandomMonsterItemDrops(roundCount);

            // Should roll for 1 item, and return it...
            var Expected = 3;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual.Count(), TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_GetRandomMonsterItemDrops_1_Roll_Level_3_Empty_ItemList_Should_Skip()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(1, 1);

            var myTurnEngine = new TurnEngine();

            // Make Sure Items List exists and is loaded...
            var myItemsViewModel = ItemsViewModel.Instance;
            var canExecute = myItemsViewModel.LoadDataCommand.CanExecute(null);
            myItemsViewModel.LoadDataCommand.Execute(null);

            // Clear the items
            MockDataStore.Instance.DeleteTables();

            var roundCount = 3;

            // Don't init itemlist so exit with empty list...
            var Actual = myTurnEngine.GetRandomMonsterItemDrops(roundCount);

            // Should roll for 1 item, and return it...
            var Expected = 1;

            // Reset
            GameGlobals.ToggleRandomState();

            // Restor the database
            MockDataStore.Instance.InitializeDatabaseNewTables();

            Assert.AreEqual(Expected, Actual.Count(), "Item Count " +TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_GetRandomMonsterItemDrops_1_Roll_Level_3_Good_ItemList_Should_Return_List()
        {
            MockForms.Init();

            // Turn off random numbers
            // Set random to 1, and to hit to 1
            GameGlobals.SetForcedRandomNumbers(2, 1);

            var myTurnEngine = new TurnEngine();

            var roundCount = 2;

            // Make Sure Items List exists and is loaded...
            var myItemsViewModel = ItemsViewModel.Instance;
            var canExecute = myItemsViewModel.LoadDataCommand.CanExecute(null);
            myItemsViewModel.LoadDataCommand.Execute(null);

            // Don't init itemlist so exit with empty list...
            var Actual = myTurnEngine.GetRandomMonsterItemDrops(roundCount);

            // Should roll for 1 item, and return it...
            var Expected = 2;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual.Count(), "Item Count " + TestContext.CurrentContext.Test.Name);
            Assert.AreEqual(roundCount, Actual.FirstOrDefault().Value, "Item Level " + TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_GetRandomMonsterItemDrops_AllowMonsterDropItems_False_Should_Skip()
        {
            MockForms.Init();

            GameGlobals.AllowMonsterDropItems = false;

            var myTurnEngine = new TurnEngine();

            var roundCount = 2;


            // Don't init itemlist so exit with empty list...
            var Actual = myTurnEngine.GetRandomMonsterItemDrops(roundCount);

            // Should roll for 1 item, and return it...
            var Expected = 0;

            // Reset
            GameGlobals.ToggleRandomState();

            Assert.AreEqual(Expected, Actual.Count(), "Item Count " + TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TurnEngine_GetRandomMonsterItemDrops_Null_ItemList_Should_Skip()
        {
            MockForms.Init();

            var myTurnEngine = new TurnEngine();

            var roundCount = 2;

            // Make Sure Items List exists and is loaded...
            var myItemsViewModel = ItemsViewModel.Instance;
            var canExecute = myItemsViewModel.LoadDataCommand.CanExecute(null);
            myItemsViewModel.LoadDataCommand.Execute(null);

            // Clear data, and load again...
            myItemsViewModel.Dataset.Clear();

            // Don't init itemlist so exit with empty list...
            var Actual = myTurnEngine.GetRandomMonsterItemDrops(roundCount);

            // Should roll for 1 item, and return it...
            var Expected = 0;

            // Reset
            GameGlobals.ToggleRandomState();
            MockDataStore.Instance.InitializeDatabaseNewTables();

            Assert.AreEqual(Expected, Actual.Count(), TestContext.CurrentContext.Test.Name);
        }

        #endregion GetRandomMonsterItemDrops
    }
}