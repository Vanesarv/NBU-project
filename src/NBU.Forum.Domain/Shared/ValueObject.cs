namespace NBU.Forum.Domain.Shared;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null,
                obj))
        {
            return false;
        }

        if (ReferenceEquals(this,
                obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return this.Equals(obj as ValueObject);
    }

    public bool Equals(ValueObject? other)
    {
        if (ReferenceEquals(null, 
                other))
        {
            return false;
        }

        if (ReferenceEquals(this,
                other))
        {
            return true;
        }

        if (other.GetType() != this.GetType())
        {
            return false;
        }

        return this.GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
        => Equals(left, right);

    public static bool operator !=(ValueObject? left, ValueObject? right)
        => !(left == right);

    public override int GetHashCode()
        => this.GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
}
