using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public readonly string Name;
        public readonly MessageType Type;
        public readonly MessageTopic Topic;
        public Category(string _name, MessageType _messageType, MessageTopic _messageTopic)
        {
            Name = _name;
            Type = _messageType;
            Topic = _messageTopic;
        }

        public static bool operator <= (Category сategory1, Category сategory2)
        {
            return сategory1.CompareTo(сategory2) <= 0;
        }
        public static bool operator >= (Category сategory1, Category сategory2)
        {
            return сategory1.CompareTo(сategory2) >= 0;
        }
        public static bool operator == (Category сategory1, Category сategory2)
        {
            return сategory1.CompareTo(сategory2) == 0;
        }
        public static bool operator != (Category сategory1, Category сategory2)
        {
            return сategory1.CompareTo(сategory2) != 0;
        }
        public static bool operator > (Category сategory1, Category сategory2)
        {
            return сategory1.CompareTo(сategory2) > 0;
        }
        public static bool operator < (Category сategory1, Category сategory2)
        {
            return сategory1.CompareTo(сategory2) < 0;
        }

        public override string ToString()
        {
            return Name + "." + Type + "." + Topic;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Category)) return 1;
            
            var other = (Category)obj;

            if (Name == null || other.Name == null)
                return 0;

            var nameComparison  = Name.CompareTo(other.Name);
            var typeComparison  = Type.CompareTo(other.Type);
            var topicComparison = Topic.CompareTo(other.Topic);
            
            if (nameComparison  != 0) return nameComparison;
            if (typeComparison  != 0) return typeComparison;
            if (topicComparison != 0) return topicComparison;
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Category) || this.GetHashCode() != obj.GetHashCode())
                return false;

            var other = (Category)obj;
            return Name == other.Name && Type == other.Type && Topic == other.Topic;
        }

        public override int GetHashCode()
        {
            if (Name == null) return base.GetHashCode();
            var hash = 13;
            hash = (hash * 7) + Name.GetHashCode();
            hash = (hash * 7) + Topic.GetHashCode();
            hash = (hash * 7) + Type.GetHashCode();
            return hash;
        }
    }
}
