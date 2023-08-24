using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200081E RID: 2078
[AddComponentMenu("uScript/Graphs/M03_Gameplay")]
public class M03_Gameplay_Component : uScriptCode
{
	// Token: 0x06005FDE RID: 24542 RVA: 0x001A92B8 File Offset: 0x001A74B8
	public M03_Gameplay_Component()
	{
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06005FDF RID: 24543 RVA: 0x001A92CC File Offset: 0x001A74CC
	// (set) Token: 0x06005FE0 RID: 24544 RVA: 0x001A92DC File Offset: 0x001A74DC
	public UnitSpawnWaveData RaiderSupportCruiserWave
	{
		get
		{
			return this.ExposedVariables.RaiderSupportCruiserWave;
		}
		set
		{
			this.ExposedVariables.RaiderSupportCruiserWave = value;
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06005FE1 RID: 24545 RVA: 0x001A92EC File Offset: 0x001A74EC
	// (set) Token: 0x06005FE2 RID: 24546 RVA: 0x001A92FC File Offset: 0x001A74FC
	public UnitSpawnWaveData ProdCruiserRespawningWaves
	{
		get
		{
			return this.ExposedVariables.ProdCruiserRespawningWaves;
		}
		set
		{
			this.ExposedVariables.ProdCruiserRespawningWaves = value;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06005FE3 RID: 24547 RVA: 0x001A930C File Offset: 0x001A750C
	// (set) Token: 0x06005FE4 RID: 24548 RVA: 0x001A931C File Offset: 0x001A751C
	public UnitSpawnWaveData RefineryCruiserStarhullSpawn
	{
		get
		{
			return this.ExposedVariables.RefineryCruiserStarhullSpawn;
		}
		set
		{
			this.ExposedVariables.RefineryCruiserStarhullSpawn = value;
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06005FE5 RID: 24549 RVA: 0x001A932C File Offset: 0x001A752C
	// (set) Token: 0x06005FE6 RID: 24550 RVA: 0x001A933C File Offset: 0x001A753C
	public QueryContainer AllRaidersQuery
	{
		get
		{
			return this.ExposedVariables.AllRaidersQuery;
		}
		set
		{
			this.ExposedVariables.AllRaidersQuery = value;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06005FE7 RID: 24551 RVA: 0x001A934C File Offset: 0x001A754C
	// (set) Token: 0x06005FE8 RID: 24552 RVA: 0x001A935C File Offset: 0x001A755C
	public UnitSpawnWaveData AmbushSkimmerSpawn
	{
		get
		{
			return this.ExposedVariables.AmbushSkimmerSpawn;
		}
		set
		{
			this.ExposedVariables.AmbushSkimmerSpawn = value;
		}
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06005FE9 RID: 24553 RVA: 0x001A936C File Offset: 0x001A756C
	// (set) Token: 0x06005FEA RID: 24554 RVA: 0x001A937C File Offset: 0x001A757C
	public UnitSpawnWaveData BlockingForces
	{
		get
		{
			return this.ExposedVariables.BlockingForces;
		}
		set
		{
			this.ExposedVariables.BlockingForces = value;
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06005FEB RID: 24555 RVA: 0x001A938C File Offset: 0x001A758C
	// (set) Token: 0x06005FEC RID: 24556 RVA: 0x001A939C File Offset: 0x001A759C
	public bool VanguardDestroyed
	{
		get
		{
			return this.ExposedVariables.VanguardDestroyed;
		}
		set
		{
			this.ExposedVariables.VanguardDestroyed = value;
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06005FED RID: 24557 RVA: 0x001A93AC File Offset: 0x001A75AC
	// (set) Token: 0x06005FEE RID: 24558 RVA: 0x001A93BC File Offset: 0x001A75BC
	public UnitSpawnWaveData GaalsiReinforcementsWave
	{
		get
		{
			return this.ExposedVariables.GaalsiReinforcementsWave;
		}
		set
		{
			this.ExposedVariables.GaalsiReinforcementsWave = value;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06005FEF RID: 24559 RVA: 0x001A93CC File Offset: 0x001A75CC
	// (set) Token: 0x06005FF0 RID: 24560 RVA: 0x001A93DC File Offset: 0x001A75DC
	public QueryContainer GameplayCarrierQuery
	{
		get
		{
			return this.ExposedVariables.GameplayCarrierQuery;
		}
		set
		{
			this.ExposedVariables.GameplayCarrierQuery = value;
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06005FF1 RID: 24561 RVA: 0x001A93EC File Offset: 0x001A75EC
	// (set) Token: 0x06005FF2 RID: 24562 RVA: 0x001A93FC File Offset: 0x001A75FC
	public QueryContainer RachelQuery
	{
		get
		{
			return this.ExposedVariables.RachelQuery;
		}
		set
		{
			this.ExposedVariables.RachelQuery = value;
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06005FF3 RID: 24563 RVA: 0x001A940C File Offset: 0x001A760C
	// (set) Token: 0x06005FF4 RID: 24564 RVA: 0x001A941C File Offset: 0x001A761C
	public UnitSpawnWaveData AmbushStarhullSpawn
	{
		get
		{
			return this.ExposedVariables.AmbushStarhullSpawn;
		}
		set
		{
			this.ExposedVariables.AmbushStarhullSpawn = value;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06005FF5 RID: 24565 RVA: 0x001A942C File Offset: 0x001A762C
	// (set) Token: 0x06005FF6 RID: 24566 RVA: 0x001A943C File Offset: 0x001A763C
	public UnitSpawnWaveData EasternGaalsienSpawn
	{
		get
		{
			return this.ExposedVariables.EasternGaalsienSpawn;
		}
		set
		{
			this.ExposedVariables.EasternGaalsienSpawn = value;
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06005FF7 RID: 24567 RVA: 0x001A944C File Offset: 0x001A764C
	// (set) Token: 0x06005FF8 RID: 24568 RVA: 0x001A945C File Offset: 0x001A765C
	public UnitSpawnWaveData TinyDerelictSpawn01
	{
		get
		{
			return this.ExposedVariables.TinyDerelictSpawn01;
		}
		set
		{
			this.ExposedVariables.TinyDerelictSpawn01 = value;
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06005FF9 RID: 24569 RVA: 0x001A946C File Offset: 0x001A766C
	// (set) Token: 0x06005FFA RID: 24570 RVA: 0x001A947C File Offset: 0x001A767C
	public UnitSpawnWaveData TinyDerelictSpawn02
	{
		get
		{
			return this.ExposedVariables.TinyDerelictSpawn02;
		}
		set
		{
			this.ExposedVariables.TinyDerelictSpawn02 = value;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06005FFB RID: 24571 RVA: 0x001A948C File Offset: 0x001A768C
	// (set) Token: 0x06005FFC RID: 24572 RVA: 0x001A949C File Offset: 0x001A769C
	public UnitSpawnWaveData TinyDerelictSpawn03
	{
		get
		{
			return this.ExposedVariables.TinyDerelictSpawn03;
		}
		set
		{
			this.ExposedVariables.TinyDerelictSpawn03 = value;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06005FFD RID: 24573 RVA: 0x001A94AC File Offset: 0x001A76AC
	// (set) Token: 0x06005FFE RID: 24574 RVA: 0x001A94BC File Offset: 0x001A76BC
	public UnitSpawnWaveData KapisiDrivebySkimmerSpawn
	{
		get
		{
			return this.ExposedVariables.KapisiDrivebySkimmerSpawn;
		}
		set
		{
			this.ExposedVariables.KapisiDrivebySkimmerSpawn = value;
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06005FFF RID: 24575 RVA: 0x001A94CC File Offset: 0x001A76CC
	// (set) Token: 0x06006000 RID: 24576 RVA: 0x001A94DC File Offset: 0x001A76DC
	public UnitSpawnWaveData KapisiDrivebyStarhullSpawn
	{
		get
		{
			return this.ExposedVariables.KapisiDrivebyStarhullSpawn;
		}
		set
		{
			this.ExposedVariables.KapisiDrivebyStarhullSpawn = value;
		}
	}

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06006001 RID: 24577 RVA: 0x001A94EC File Offset: 0x001A76EC
	// (set) Token: 0x06006002 RID: 24578 RVA: 0x001A94FC File Offset: 0x001A76FC
	public UnitSpawnWaveData TinyDerelictSpawn04
	{
		get
		{
			return this.ExposedVariables.TinyDerelictSpawn04;
		}
		set
		{
			this.ExposedVariables.TinyDerelictSpawn04 = value;
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06006003 RID: 24579 RVA: 0x001A950C File Offset: 0x001A770C
	// (set) Token: 0x06006004 RID: 24580 RVA: 0x001A951C File Offset: 0x001A771C
	public UnitSpawnWaveData TinyDerelictSpawn05
	{
		get
		{
			return this.ExposedVariables.TinyDerelictSpawn05;
		}
		set
		{
			this.ExposedVariables.TinyDerelictSpawn05 = value;
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06006005 RID: 24581 RVA: 0x001A952C File Offset: 0x001A772C
	// (set) Token: 0x06006006 RID: 24582 RVA: 0x001A953C File Offset: 0x001A773C
	public UnitSpawnWaveData TinyDerelictSpawn06
	{
		get
		{
			return this.ExposedVariables.TinyDerelictSpawn06;
		}
		set
		{
			this.ExposedVariables.TinyDerelictSpawn06 = value;
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06006007 RID: 24583 RVA: 0x001A954C File Offset: 0x001A774C
	// (set) Token: 0x06006008 RID: 24584 RVA: 0x001A955C File Offset: 0x001A775C
	public UnitSpawnWaveData RefineryCruiserReactionarySkimmerSpawn
	{
		get
		{
			return this.ExposedVariables.RefineryCruiserReactionarySkimmerSpawn;
		}
		set
		{
			this.ExposedVariables.RefineryCruiserReactionarySkimmerSpawn = value;
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06006009 RID: 24585 RVA: 0x001A956C File Offset: 0x001A776C
	// (set) Token: 0x0600600A RID: 24586 RVA: 0x001A957C File Offset: 0x001A777C
	public AttributeBuffSetData MovementSpeedAndArmourBuff
	{
		get
		{
			return this.ExposedVariables.MovementSpeedAndArmourBuff;
		}
		set
		{
			this.ExposedVariables.MovementSpeedAndArmourBuff = value;
		}
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x0600600B RID: 24587 RVA: 0x001A958C File Offset: 0x001A778C
	// (set) Token: 0x0600600C RID: 24588 RVA: 0x001A959C File Offset: 0x001A779C
	public QueryContainer CheckPlayerCombatUnitCount
	{
		get
		{
			return this.ExposedVariables.CheckPlayerCombatUnitCount;
		}
		set
		{
			this.ExposedVariables.CheckPlayerCombatUnitCount = value;
		}
	}

	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x0600600D RID: 24589 RVA: 0x001A95AC File Offset: 0x001A77AC
	// (set) Token: 0x0600600E RID: 24590 RVA: 0x001A95BC File Offset: 0x001A77BC
	public QueryContainer EmptyQuery
	{
		get
		{
			return this.ExposedVariables.EmptyQuery;
		}
		set
		{
			this.ExposedVariables.EmptyQuery = value;
		}
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x0600600F RID: 24591 RVA: 0x001A95CC File Offset: 0x001A77CC
	// (set) Token: 0x06006010 RID: 24592 RVA: 0x001A95DC File Offset: 0x001A77DC
	public UnitSpawnWaveData CarrierWave
	{
		get
		{
			return this.ExposedVariables.CarrierWave;
		}
		set
		{
			this.ExposedVariables.CarrierWave = value;
		}
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06006011 RID: 24593 RVA: 0x001A95EC File Offset: 0x001A77EC
	// (set) Token: 0x06006012 RID: 24594 RVA: 0x001A95FC File Offset: 0x001A77FC
	public UnitSpawnWaveData CarrierGuardWave
	{
		get
		{
			return this.ExposedVariables.CarrierGuardWave;
		}
		set
		{
			this.ExposedVariables.CarrierGuardWave = value;
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06006013 RID: 24595 RVA: 0x001A960C File Offset: 0x001A780C
	// (set) Token: 0x06006014 RID: 24596 RVA: 0x001A961C File Offset: 0x001A781C
	public AttributeBuffSetData SmallSpeedBoost
	{
		get
		{
			return this.ExposedVariables.SmallSpeedBoost;
		}
		set
		{
			this.ExposedVariables.SmallSpeedBoost = value;
		}
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06006015 RID: 24597 RVA: 0x001A962C File Offset: 0x001A782C
	// (set) Token: 0x06006016 RID: 24598 RVA: 0x001A963C File Offset: 0x001A783C
	public QueryContainer ExpeditionUnits
	{
		get
		{
			return this.ExposedVariables.ExpeditionUnits;
		}
		set
		{
			this.ExposedVariables.ExpeditionUnits = value;
		}
	}

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06006017 RID: 24599 RVA: 0x001A964C File Offset: 0x001A784C
	// (set) Token: 0x06006018 RID: 24600 RVA: 0x001A965C File Offset: 0x001A785C
	public AttributeBuffSetData SensorDebuff
	{
		get
		{
			return this.ExposedVariables.SensorDebuff;
		}
		set
		{
			this.ExposedVariables.SensorDebuff = value;
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06006019 RID: 24601 RVA: 0x001A966C File Offset: 0x001A786C
	// (set) Token: 0x0600601A RID: 24602 RVA: 0x001A967C File Offset: 0x001A787C
	public QueryContainer PlayerCombatUnitsAndCarrier
	{
		get
		{
			return this.ExposedVariables.PlayerCombatUnitsAndCarrier;
		}
		set
		{
			this.ExposedVariables.PlayerCombatUnitsAndCarrier = value;
		}
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x0600601B RID: 24603 RVA: 0x001A968C File Offset: 0x001A788C
	// (set) Token: 0x0600601C RID: 24604 RVA: 0x001A969C File Offset: 0x001A789C
	public UnitSpawnWaveData SingleRailgun
	{
		get
		{
			return this.ExposedVariables.SingleRailgun;
		}
		set
		{
			this.ExposedVariables.SingleRailgun = value;
		}
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x0600601D RID: 24605 RVA: 0x001A96AC File Offset: 0x001A78AC
	// (set) Token: 0x0600601E RID: 24606 RVA: 0x001A96BC File Offset: 0x001A78BC
	public AttributeBuffSetData MovementSpeedBuff
	{
		get
		{
			return this.ExposedVariables.MovementSpeedBuff;
		}
		set
		{
			this.ExposedVariables.MovementSpeedBuff = value;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x0600601F RID: 24607 RVA: 0x001A96CC File Offset: 0x001A78CC
	// (set) Token: 0x06006020 RID: 24608 RVA: 0x001A96DC File Offset: 0x001A78DC
	public AttributeBuffSetData NoAutoTargetAcquisition
	{
		get
		{
			return this.ExposedVariables.NoAutoTargetAcquisition;
		}
		set
		{
			this.ExposedVariables.NoAutoTargetAcquisition = value;
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06006021 RID: 24609 RVA: 0x001A96EC File Offset: 0x001A78EC
	// (set) Token: 0x06006022 RID: 24610 RVA: 0x001A96FC File Offset: 0x001A78FC
	public AttributeBuffSetData NoAutoTargetEnemies
	{
		get
		{
			return this.ExposedVariables.NoAutoTargetEnemies;
		}
		set
		{
			this.ExposedVariables.NoAutoTargetEnemies = value;
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06006023 RID: 24611 RVA: 0x001A970C File Offset: 0x001A790C
	// (set) Token: 0x06006024 RID: 24612 RVA: 0x001A971C File Offset: 0x001A791C
	public int RepairedRailguns
	{
		get
		{
			return this.ExposedVariables.RepairedRailguns;
		}
		set
		{
			this.ExposedVariables.RepairedRailguns = value;
		}
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06006025 RID: 24613 RVA: 0x001A972C File Offset: 0x001A792C
	// (set) Token: 0x06006026 RID: 24614 RVA: 0x001A973C File Offset: 0x001A793C
	public AttributeBuffSetData ProdCruiserHealthBuff
	{
		get
		{
			return this.ExposedVariables.ProdCruiserHealthBuff;
		}
		set
		{
			this.ExposedVariables.ProdCruiserHealthBuff = value;
		}
	}

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06006027 RID: 24615 RVA: 0x001A974C File Offset: 0x001A794C
	// (set) Token: 0x06006028 RID: 24616 RVA: 0x001A975C File Offset: 0x001A795C
	public QueryContainer AllCommanders
	{
		get
		{
			return this.ExposedVariables.AllCommanders;
		}
		set
		{
			this.ExposedVariables.AllCommanders = value;
		}
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06006029 RID: 24617 RVA: 0x001A976C File Offset: 0x001A796C
	// (set) Token: 0x0600602A RID: 24618 RVA: 0x001A977C File Offset: 0x001A797C
	public AttributeBuffSetData ProductionCruiserStarhullTempBuff
	{
		get
		{
			return this.ExposedVariables.ProductionCruiserStarhullTempBuff;
		}
		set
		{
			this.ExposedVariables.ProductionCruiserStarhullTempBuff = value;
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x0600602B RID: 24619 RVA: 0x001A978C File Offset: 0x001A798C
	// (set) Token: 0x0600602C RID: 24620 RVA: 0x001A979C File Offset: 0x001A799C
	public AttributeBuffSetData LongLeashBuff
	{
		get
		{
			return this.ExposedVariables.LongLeashBuff;
		}
		set
		{
			this.ExposedVariables.LongLeashBuff = value;
		}
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x0600602D RID: 24621 RVA: 0x001A97AC File Offset: 0x001A79AC
	// (set) Token: 0x0600602E RID: 24622 RVA: 0x001A97BC File Offset: 0x001A79BC
	public AttributeBuffSetData ShortLeashDebuff
	{
		get
		{
			return this.ExposedVariables.ShortLeashDebuff;
		}
		set
		{
			this.ExposedVariables.ShortLeashDebuff = value;
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x0600602F RID: 24623 RVA: 0x001A97CC File Offset: 0x001A79CC
	// (set) Token: 0x06006030 RID: 24624 RVA: 0x001A97DC File Offset: 0x001A79DC
	public UnitSpawnWaveData RefineryCruiserReactionaryCatSpawn
	{
		get
		{
			return this.ExposedVariables.RefineryCruiserReactionaryCatSpawn;
		}
		set
		{
			this.ExposedVariables.RefineryCruiserReactionaryCatSpawn = value;
		}
	}

	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06006031 RID: 24625 RVA: 0x001A97EC File Offset: 0x001A79EC
	// (set) Token: 0x06006032 RID: 24626 RVA: 0x001A97FC File Offset: 0x001A79FC
	public AttributeBuffSetData StarhullWeaponRangeDebuff
	{
		get
		{
			return this.ExposedVariables.StarhullWeaponRangeDebuff;
		}
		set
		{
			this.ExposedVariables.StarhullWeaponRangeDebuff = value;
		}
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06006033 RID: 24627 RVA: 0x001A980C File Offset: 0x001A7A0C
	// (set) Token: 0x06006034 RID: 24628 RVA: 0x001A981C File Offset: 0x001A7A1C
	public QueryContainer PlayerSupportCruiserQuery
	{
		get
		{
			return this.ExposedVariables.PlayerSupportCruiserQuery;
		}
		set
		{
			this.ExposedVariables.PlayerSupportCruiserQuery = value;
		}
	}

	// Token: 0x06006035 RID: 24629 RVA: 0x001A982C File Offset: 0x001A7A2C
	private void Awake()
	{
		base.useGUILayout = false;
		this.ExposedVariables.Awake();
		this.ExposedVariables.SetParent(base.gameObject);
		if ("1.CMR" != uScript_MasterComponent.Version)
		{
			uScriptDebug.Log("The generated code is not compatible with your current uScript Runtime " + uScript_MasterComponent.Version, uScriptDebug.Type.Error);
			this.ExposedVariables = null;
			Debug.Break();
		}
	}

	// Token: 0x06006036 RID: 24630 RVA: 0x001A9894 File Offset: 0x001A7A94
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06006037 RID: 24631 RVA: 0x001A98A4 File Offset: 0x001A7AA4
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06006038 RID: 24632 RVA: 0x001A98B4 File Offset: 0x001A7AB4
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06006039 RID: 24633 RVA: 0x001A98C4 File Offset: 0x001A7AC4
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600603A RID: 24634 RVA: 0x001A98D4 File Offset: 0x001A7AD4
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400845C RID: 33884
	public M03_Gameplay ExposedVariables = new M03_Gameplay();
}
