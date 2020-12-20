using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Brighton.Data
{

    public class StaticCalculatedField<T> : CalculatedField<T>
    {
        private T _Value;

        public StaticCalculatedField(string Name, T Value)
            : base(Name)
        {
            _Value = Value;
        }
        public override object Value
        {
            get { return _Value; }
        }
    }

    public abstract class CalculatedField<T> : ICalculatedField
    {
        string _Name;

        public CalculatedField(string Name)
        {
            _Name = Name;
        }

        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public Type FieldType
        {
            get
            {
                return typeof(T);
            }
        }

        public abstract object Value { get; }
    }

    public interface ICalculatedField
    {
        string Name { get;}
        Type FieldType { get;}
        object Value { get; }
    }

    public class CalculatedFieldCollection : List<ICalculatedField>
    {
        public CalculatedFieldCollection() : base()
        {
        }
    }

    public class CalculatedFieldReader : IDataReader
    {
        private IDataReader BaseReader;
        private ICalculatedField[] Fields;


        public CalculatedFieldReader(IDataReader reader, CalculatedFieldCollection CalculatedFields)
        {
            Fields = new ICalculatedField[CalculatedFields.Count];
            int i = 0;
            foreach (ICalculatedField fld in CalculatedFields)
            {
                Fields[i] = fld;
                i++;
            }
            BaseReader = reader;
        }

        private ICalculatedField GetCalculatedField(int i, Type ExpectedType)
        {
            if (i < BaseReader.FieldCount)
                return null;
            i = i - BaseReader.FieldCount;
            if (i > Fields.Length - 1)
                throw new IndexOutOfRangeException();
            ICalculatedField fld = Fields[i];
            if (ExpectedType!=null && fld.FieldType != ExpectedType)
                throw new InvalidCastException();
            return fld;
        }

        #region IDataReader Members

        public void Close()
        {
            BaseReader.Close();
        }

        public int Depth
        {
            get { return BaseReader.Depth; }
        }

        public DataTable GetSchemaTable()
        {
            DataTable dt = BaseReader.GetSchemaTable();
            int i = BaseReader.FieldCount;
            //TODO: This only provides the basics
            foreach (ICalculatedField fld in Fields)
            {
                DataRow row=dt.NewRow();
                row["ColumnName"] = fld.Name;
                row["ColumnOrdinal"] = i;
                //row["ColumnSize"];
                row["IsUnique"] = false;
                row["BaseColumnName"] = fld.Name;
                row["DataType"] = fld.FieldType;
                row["AllowDBNull"] = true;
                dt.Rows.Add(row);
                i++;
            }
            return dt;
        }

        public bool IsClosed
        {
            get { return BaseReader.IsClosed; }
        }

        public bool NextResult()
        {
            return BaseReader.NextResult();
        }

        public bool Read()
        {
            return BaseReader.Read();
        }

        public int RecordsAffected
        {
            get { return BaseReader.RecordsAffected; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            BaseReader.Dispose();
        }

        #endregion

        #region IDataRecord Members

        public int FieldCount
        {
            get { return BaseReader.FieldCount + Fields.Length; }
        }
        
        public bool GetBoolean(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(bool));
            if (fld==null)
                return (bool)fld.Value;
            else
                return BaseReader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(byte));
            if (fld != null)
                return (byte)fld.Value;
            else
                return BaseReader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public char GetChar(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(char));
            if (fld != null)
                return (char)fld.Value;
            else
                return BaseReader.GetChar(i);
        }

        public IDataReader GetData(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(IDataReader));
            if (fld != null)
                return (IDataReader)fld.Value;
            else
                return BaseReader.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, null);
            if (fld != null)
                // TODO: Is this correct?
                return fld.FieldType.Name;
            else
                return BaseReader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(DateTime));
            if (fld != null)
                return (DateTime)fld.Value;
            else
                return BaseReader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(decimal));
            if (fld != null)
                return (decimal)fld.Value;
            else
                return BaseReader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(double));
            if (fld != null)
                return (double)fld.Value;
            else
                return BaseReader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(double));
            if (fld != null)
                return fld.FieldType;
            else
                return BaseReader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(float));
            if (fld != null)
                return (float)fld.Value;
            else
                return BaseReader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(Guid));
            if (fld != null)
                return (Guid)fld.Value;
            else
                return BaseReader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(short));
            if (fld != null)
                return (short)fld.Value;
            else
                return BaseReader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(int));
            if (fld != null)
                return (int)fld.Value;
            else
                return BaseReader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(long));
            if (fld != null)
                return (long)fld.Value;
            else
                return BaseReader.GetInt64(i);
        }

        public string GetName(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, null);
            if (fld != null)
                return fld.Name;
            else
                return BaseReader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < Fields.Length; i++)
            {
                if (Fields[i].Name == name)
                    return BaseReader.FieldCount+i;
            }
            return BaseReader.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, typeof(string));
            if (fld != null)
                return (string) fld.Value;
            else
                return BaseReader.GetString(i);
        }

        public object GetValue(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, null);
            if (fld != null)
                return fld.Value;
            else
                return BaseReader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < FieldCount; i++)
                values[i] = GetValue(i);
            return FieldCount;
        }

        public bool IsDBNull(int i)
        {
            ICalculatedField fld = GetCalculatedField(i, null);
            if (fld != null)
                return fld.Value is DBNull;
            else
                return BaseReader.IsDBNull(i);
        }

        public object this[string name]
        {
            get 
            {
                for (int i = 0; i < Fields.Length; i++)
                {
                    if (Fields[i].Name == name)
                        return Fields[i].Value;
                }
                return BaseReader[name];
            }
        }

        public object this[int i]
        {
            get
            {
                if (i > BaseReader.FieldCount - 1)
                {
                    return GetCalculatedField(i, null);
                }
                return BaseReader[i];
            }
        }

        #endregion
    }
}