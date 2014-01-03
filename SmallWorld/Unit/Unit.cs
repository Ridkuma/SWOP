using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    public class Unit : IUnit
    {
        public ITile Position { get; set; }
        public int Atk { get; protected set; }
        public int Def { get; protected set; }
        public int Hp { get; set; }
        public int HpMax { get; protected set; }
        public int Mvt { get; protected set; }
        public string Name { get; protected set; }
        public UnitState State { get; set; }
        public FactionName Faction { get; protected set; }


        public Unit(string name, ITile position)
        {
            this.Atk = 2;
            this.Def = 1;
            this.HpMax = 2;
            this.Hp = this.HpMax;
            this.Mvt = 1;
            this.Name = name;
            this.State = UnitState.Idle;
            this.Position = position;
            position.UnitEnter(this);
        }

        #region MoveLogic

        /// <summary>
        /// Default check whether a Unit can or not move to destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public virtual bool CheckMove(ITile destination)
        {
            // Minimum verifications before allowing a move,
            // every Unit extension should add up their own limitations
            bool possibleMove = (this.Mvt > 0)
                && GameMaster.GM.CurrentGame.CurrentPlayerIsMe
                && (!destination.IsOccupiedByEnnemy(this))
                && (destination != this.Position);

            return possibleMove;
        }

        /// <summary>
        /// Ask IGame if move is possible (server check in network game)
        /// </summary>
        /// <param name="destination"></param>
        public virtual void Move(ITile destination)
        {
            GameMaster.GM.CurrentGame.MoveUnit(this, destination);
        }

        /// <summary>
        /// Effectively move a Unit to destination (ordered by IGame)
        /// </summary>
        /// <param name="destination"></param>
        public virtual void RealMove(ITile destination)
        {
            this.Position.UnitLeave(this);
            this.Position = destination;
            destination.UnitEnter(this);
        }

        #endregion

        #region CombatLogic

        /// <summary>
        /// Check whether a Unit can attack another
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public bool CheckAttack(IUnit enemy)
        {
            return this.Position.IsAdjacent(enemy.Position)
                && this.Mvt > 0
                && enemy.Faction != this.Faction
                && GameMaster.GM.CurrentGame.CurrentPlayerIsMe;
        }

        /// <summary>
        /// Ask IGame if attacking is possible (server check in network game)
        /// </summary>
        /// <param name="enemy"></param>
        public void Attack(IUnit enemy)
        {
            if (!this.CheckAttack(enemy))
                return;

            GameMaster.GM.CurrentGame.AttackUnit(this, enemy);
        }

        /// <summary>
        /// Effectively starts a combat with enemy
        /// Attacking is the same for every faction
        /// </summary>
        /// <param name="enemy"></param>
        public void RealAttack(IUnit enemy)
        {
            this.ChangeState(UnitState.Attacking);
            enemy.ChangeState(UnitState.Defending);
            this.Mvt--;
            GameMaster.GM.CurrentGame.Fight(this, enemy);

            // Check wounds after fighting
            if (this.Hp == 0)
                this.ChangeState(UnitState.Dead);
            else
                this.ChangeState(UnitState.Idle);

            if (enemy.Hp == 0)
                enemy.ChangeState(UnitState.Dead);
            else
                enemy.ChangeState(UnitState.Idle);
        }

        #endregion

        /// <summary>
        /// Change current state
        /// </summary>
        /// <param name="targetState"></param>
        public void ChangeState(UnitState targetState)
        {
            switch (this.State)
            {
                case UnitState.Idle :
                    switch (targetState)
                    {
                        case UnitState.Selected :
                            this.State = targetState;
                            break;

                        case UnitState.Defending :
                            this.State = targetState;
                            break;
                    }
                    break;

                case UnitState.Selected :
                    switch (targetState)
                    {
                        case UnitState.Move :
                            this.State = targetState;
                            break;

                        case UnitState.Attacking :
                            this.State = targetState;
                            break;

                        case UnitState.Idle :
                            this.State = targetState;
                            break;
                    }
                    break;

                case UnitState.Move:
                    switch (targetState)
                    {
                        case UnitState.Idle:
                            this.State = targetState;
                            break;

                    }
                    break;

                case UnitState.Attacking:
                    switch (targetState)
                    {
                        case UnitState.Move:
                            this.State = targetState;
                            break;

                        case UnitState.Idle:
                            this.State = targetState;
                            break;

                        case UnitState.Dead:
                            this.State = targetState;
                            break;
                    }
                    break;

                case UnitState.Defending:
                    switch (targetState)
                    {
                        case UnitState.Idle:
                            this.State = targetState;
                            break;

                        case UnitState.Dead:
                            this.State = targetState;
                            break;
                    }
                    break;

            }
        }

        /// <summary>
        /// Default actions on turn end
        /// </summary>
        public virtual void EndTurn()
        {
            this.Mvt = 1;
        }

    }


    public class VikingsUnit : Unit
    {
        public VikingsUnit(string name, ITile position) 
            : base(name, position)
        {
            this.Faction = FactionName.Vikings;
        }


        public override bool CheckMove(ITile destination)
        {
            if (!destination.IsAdjacent(this.Position))
                return false;

            return base.CheckMove(destination);
        }

        public override void Move(ITile destination)
        {
            if (!this.CheckMove(destination))
                return;
            this.Mvt--;
            base.Move(destination);
        }
    }

    public class DwarvesUnit : Unit
    {
        public DwarvesUnit(string name, ITile position) 
            : base(name, position)
        {
            this.Faction = FactionName.Dwarves;
        }

        /// <summary>
        /// A Dwarf can move from a Mountain to any other Mountain
        /// Cannot cross Water
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public override bool CheckMove(ITile destination)
        {
            bool mountainTravel = destination.Type == TileType.Mountain
                && this.Position.Type == TileType.Mountain;

            if (!destination.IsAdjacent(this.Position) || destination.Type == TileType.Water)
            {
                if (!mountainTravel)
                    return false;
            }

            return base.CheckMove(destination); ;
        }

        public override void Move(ITile destination)
        {
            if (!this.CheckMove(destination))
                return;
            this.Mvt--;
            base.Move(destination);
        }
    }

    public class GaulsUnit : Unit
    {
        public GaulsUnit(string name, ITile position) 
            : base(name, position)
        {
            this.Faction = FactionName.Gauls;
            this.Mvt = 2;
        }

        /// <summary>
        /// A Gaul can move twice if it crosses a Plain
        /// Cannot cross Water
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public override bool CheckMove(ITile destination)
        {
            if (!destination.IsAdjacent(this.Position) || destination.Type == TileType.Water)
                return false;

            return base.CheckMove(destination); ;
        }

        public override void Move(ITile destination)
        {
            if (!this.CheckMove(destination))
                return;

            if (destination.Type == TileType.Field)
                this.Mvt -= 1;
            else
                this.Mvt -= 2;

            base.Move(destination);
        }

        public override void EndTurn()
        {
            this.Mvt = 2;
        }
    }
}
