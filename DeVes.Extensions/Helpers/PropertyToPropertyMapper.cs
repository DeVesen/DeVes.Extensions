using System;
using System.Collections.Generic;
using System.Linq;

namespace DeVes.Extensions.Helpers
{
    public delegate TVResult OnActionGetValueOf<in TItem, TValue, out TVResult>(TItem sourceItem, out TValue output);
    public delegate TVResult OnActionSetValueOf<in TItem, in TValue, out TVResult>(TItem destinationItem, TValue output);


    public class PropertyToPropertyMapper
    {
        public readonly List<IPtpmMover> Mover = new List<IPtpmMover>();
        public readonly List<PtpmItem> MappingItems = new List<PtpmItem>();


        public OnActionGetValueOf<PtpmItemProperty, object, bool> ActionGetValueOf;
        public OnActionSetValueOf<PtpmItemProperty, object, bool> ActionSetValueTo;

        public PtpmActionMover ActionMover => new PtpmActionMover(ActionGetValueOf, ActionSetValueTo);
        public IPtpmMover[] MoversAll
        {
            get
            {
                var _movers = new List<IPtpmMover>();

                if (ActionMover.ActionGetValueOf != null || ActionMover.ActionSetValueTo != null)
                    _movers.Add(ActionMover);

                _movers.AddRange(Mover);

                return _movers.ToArray();
            }
        }

        public void AddMover(IPtpmMover mover)
        {
            if (mover == null) return;
            Mover.Add(mover);
        }

        public void AddMap(PtpmItem item)
        {
            if (item?.Property1.Property == null) return;
            if (item.Property2.Property == null) return;

            MappingItems.Add(item);
        }
        public void AddMap(object property1, object property2)
        {
            AddMap(new PtpmItem(property1, property2));
        }
        public void AddMap(object property1, object property2, string property2Key)
        {
            AddMap(new PtpmItem(property1, property2, property2Key));
        }
        public void AddMap(object property1, string property1Key, object property2, string property2Key)
        {
            AddMap(new PtpmItem(property1, property1Key, property2, property2Key));
        }


        public void Move1To2()
        {
            foreach (var _item in MappingItems)
            {
                _item.SetProp1ToProp2(MoversAll);
            }
        }
        public void Move2To1()
        {
            foreach (var _item in MappingItems)
            {
                _item.SetProp2ToProp1(MoversAll);
            }
        }
    }

    public class PtpmItem
    {
        public PtpmItemProperty Property1 { get; }
        public PtpmItemProperty Property2 { get; }

        public bool IsDifferent => HasDifference();


        public PtpmItem(object property1, object property2)
            : this(property1, null, property2, null)
        {
        }
        public PtpmItem(object property1, object property2, string property2Key)
            : this(property1, null, property2, property2Key)
        {
        }
        public PtpmItem(object property1, string property1Key, object property2, string property2Key)
        {
            Property1 = new PtpmItemProperty(property1Key, property1);
            Property2 = new PtpmItemProperty(property2Key, property2);
        }


        public void SetProp1ToProp2(IEnumerable<IPtpmMover> movers)
        {
            MovePropertyToProperty(Property1, Property2, movers);
        }
        public void SetProp2ToProp1(IEnumerable<IPtpmMover> movers)
        {
            MovePropertyToProperty(Property2, Property1, movers);
        }

        private bool HasDifference()
        {
            return Property1 != Property2;
        }


        private static void MovePropertyToProperty(PtpmItemProperty source, PtpmItemProperty destination, IEnumerable<IPtpmMover> movers)
        {
            var _valueToMove = source.PropertyValue;
            var _ptpmMovers = movers as IPtpmMover[] ?? movers.ToArray();

            foreach (var _move in _ptpmMovers)
            {
                if (_move.TryGetValue(source, out object _newValue))
                {
                    _valueToMove = _newValue;
                }
            }
            if (_ptpmMovers.Any(move => move.TrySetValue(destination, _valueToMove)))
            {
                return;
            }

            destination.PropertyValue = _valueToMove;
        }
    }

    public class PtpmItemProperty : IComparable<PtpmItemProperty>
    {
        public string PropertyKey { get; }
        public object Property { get; private set; }
        public Type PropertyType => GetPropertyType();
        public object PropertyValue
        {
            get { return GetProperty1Value(); }
            set { SetProperty1Value(value); }
        }

        public PtpmItemProperty(string key, object property)
        {
            PropertyKey = key;
            Property = property;
        }

        private Type GetPropertyType()
        {
            return Property?.GetType() ?? typeof(object);
        }
        private object GetProperty1Value()
        {
            if (Property == null) return null;

            return string.IsNullOrEmpty(PropertyKey) ?
                Property :
                Property.GetPropertyValue<object>(PropertyKey);
        }
        private void SetProperty1Value(object value)
        {
            if (string.IsNullOrEmpty(PropertyKey))
            {
                Property = value;
                return;
            }
            Property.SetPropertyValue(PropertyKey, value);
        }

        public static bool operator ==(PtpmItemProperty property1, PtpmItemProperty property2)
        {
            if (property1 != null && property2 == null) return false;
            if (property1 == null && property2 != null) return false;
            return property1?.CompareTo(property2) == 0;
        }
        public static bool operator !=(PtpmItemProperty property1, PtpmItemProperty property2)
        {
            if (property1 != null && property2 == null) return true;
            if (property1 == null && property2 != null) return true;
            return property1?.CompareTo(property2) != 0;
        }

        public int CompareTo(PtpmItemProperty other)
        {
            var _p1 = Convert.ToString(PropertyValue);
            var _p2 = Convert.ToString(other?.PropertyValue);
            return string.Compare(_p1, _p2, StringComparison.Ordinal);
        }
        protected bool Equals(PtpmItemProperty other)
        {
            if (other == null) return false;
            return CompareTo(other) == 0;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PtpmItemProperty)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return ((PropertyKey?.GetHashCode() ?? 0) * 397) ^ (Property?.GetHashCode() ?? 0);
            }
        }
    }

    public interface IPtpmMover
    {
        bool TryGetValue(PtpmItemProperty sourceItem, out object value);
        bool TrySetValue(PtpmItemProperty destinationItem, object value);
    }

    public class PtpmActionMover : IPtpmMover
    {
        public OnActionGetValueOf<PtpmItemProperty, object, bool> ActionGetValueOf;
        public OnActionSetValueOf<PtpmItemProperty, object, bool> ActionSetValueTo;

        public bool TryGetValue(PtpmItemProperty sourceItem, out object value)
        {
            if (ActionGetValueOf != null)
                return ActionGetValueOf(sourceItem, out value);

            value = null;

            return false;
        }
        public bool TrySetValue(PtpmItemProperty destinationItem, object value)
        {
            return ActionSetValueTo != null && ActionSetValueTo(destinationItem, value);
        }


        public PtpmActionMover(OnActionGetValueOf<PtpmItemProperty, object, bool> actionGetValueOf,
                               OnActionSetValueOf<PtpmItemProperty, object, bool> actionSetValueTo)
        {
            ActionGetValueOf = actionGetValueOf;
            ActionSetValueTo = actionSetValueTo;
        }
    }
}
