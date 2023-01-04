using System.Reflection;

namespace Utilities.Reflection
{
	public static class Fields
    {
        private static readonly Dictionary<Type, FieldInfo[]> fieldsByType = new Dictionary<Type, FieldInfo[]>();

        private static readonly Dictionary<Type, FieldInfo[]> serializedFields = new Dictionary<Type, FieldInfo[]>();

        public static FieldInfo[] GetSerializableFields(this Type targetType)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            //Get the type from the fieldInfo
            //this fieldInfo is already given to you by PropertyDrawer
            //Type type = fieldInfo.FieldType;
            if (!serializedFields.TryGetValue(targetType, out FieldInfo[]? fields))
            {
                //if you don't already have the fields, go get them
                fields = targetType.GetFields(bindingFlags);
                //Make a list so we can easily remove not serialized fields
                List<FieldInfo> fieldsList = new List<FieldInfo>();
                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo field = fields[i];
                    if (!field.IsNotSerialized) fieldsList.Add(field);
                }

                //now that we have all the fields we want to display in out list, we just save it in our static dictionary
                fields = fieldsList.ToArray();
                serializedFields.Add(targetType, fields);
            }

            //Finally, if we already had the fields in our dictionary or just added them, return our fields
            return fields;
        }
    }
}
