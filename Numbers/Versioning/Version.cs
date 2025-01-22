using System.Text.RegularExpressions;

namespace Utils.Versioning
{
	public enum VersionPhase : byte
	{
		Invalid,
		Prototype,
		Alpha,
		Beta,
		Candidate,
		Release
	}

	public readonly struct Version
	{
		private const int stateShift = 24;
		private const int majorShift = 16;
		private const int minorShift = 8;
		private const byte mask = 0xFF;

		private const string regex = @"([0-9]+)\.([0-9]+)\.([0-9]+)([abpr]?)";
		private const string format = "{0}.{1}.{2}{3}";
		private static readonly Regex regexEval = new Regex(regex);

		public static bool TryParse(string value, out Version version)
		{
			if (string.IsNullOrEmpty(value))
			{
				version = default;
				return false;
			}

			Match result = regexEval.Match(value);
			if (!result.Success)
			{
				version = default;
				return false;
			}

			GroupCollection groups = result.Groups;
			byte major = byte.Parse(groups[1].Value);
			byte minor = byte.Parse(groups[2].Value);
			byte patch = byte.Parse(groups[3].Value);
			VersionPhase state = groups[4].Value switch 
			{
				"p" => VersionPhase.Prototype,
				"a" => VersionPhase.Alpha,
				"b" => VersionPhase.Beta,
				"c" => VersionPhase.Candidate,
				null or "" => VersionPhase.Release,
				_ => VersionPhase.Invalid
			};

			if (state == VersionPhase.Invalid)
			{
				version = default;
				return false;
			}

			version = new Version(state, major, minor, patch);
			return true;
		}

		public static implicit operator Version(int hash) => new Version(hash); 
		public static implicit operator int(Version ver) => ver.hash; 

		public static bool operator ==(Version a, Version b) => a.hash == b.hash;
		public static bool operator !=(Version a, Version b) => a.hash != b.hash;
		public static bool operator >(Version a, Version b) => a.hash > b.hash;
		public static bool operator <(Version a, Version b) => a.hash < b.hash;

		public readonly VersionPhase State => (VersionPhase)(byte)(hash >> stateShift);
		public readonly int Major => (hash >> majorShift) & mask;
		private readonly int Minor => (hash >> minorShift) & mask;
		public readonly int Patch => hash & mask;
		public readonly int hash;

		public Version(VersionPhase state, byte major = 0, byte minor = 0, byte patch = 0)
		{
			hash = ((byte)state << stateShift) | (major << majorShift) | (minor << minorShift) | patch;
		}
		public Version(int value)
		{
			this.hash = value;
		}

		public override int GetHashCode()
		{
			return hash;
		}
		public override bool Equals(object obj)
		{
			return obj is Version ver && ver.hash == hash;
		}
		public override string ToString()
		{
			if (State == VersionPhase.Invalid)
				return "Invalid";

			return string.Format(format, Major, Minor, Patch, State switch
			{
				VersionPhase.Prototype => "p",
				VersionPhase.Alpha => "a",
				VersionPhase.Beta => "b",
				VersionPhase.Candidate => "c",
				_ => null
			});
		}
	}
}
