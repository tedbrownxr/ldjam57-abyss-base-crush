using DevKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public class Constants 
	{
		public static bool TurretShowTargetLine = false;

		public static int BuildingHitPoints = 1000;

		public static int EnemiesPerWave = 10;
		public static int IncreasedEnemiesPerWave = 10;
		public static float SpawnDelay = 1;
		public static float SpawnDelayMin = 0.05f;
		public static float SpawnDelayAdjustPerWave = 0.1f;

		public static int EnemyMaxLevel = 2;
		public static int WavesToIncreaseEnemyLevel = 3;
		public static int EnemyHitPoints = 40;
		public static int EnemyDamage = 200;
		public static int EnemyDamageBonusPerLevel = 50;
		public static float EnemyAttackCooldown = 2;
		public static float EnemyWanderMin = 16;
		public static float EnemyWanderMax = 20;
		public static float EnemyWanderDuration = 3;
		public static float EnemyFleeDistance = 100;
		public static float EnemyMoveSpeed = 5;

		public static float GunFireCooldown = 1f;
		public static int GunPenetration = 0;
		public static int GunDamage = 10;
		public static float GunTurnSpeed = 90;

		public static float UnitMoveSpeed = 5;

	}
}
