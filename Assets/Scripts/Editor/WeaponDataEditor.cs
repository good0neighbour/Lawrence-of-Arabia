using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    private WeaponData _weaponData = null;
    private WeaponData.Weapon[] _weaponArray = null;


    private void OnEnable()
    {
        _weaponData = (WeaponData)target;
        _weaponArray = _weaponData.GetWeaponList();
        if (_weaponArray.Length != (int)CharacterWeapons.None)
        {
            WeaponData.Weapon[] newArray = new WeaponData.Weapon[(int)CharacterWeapons.None];
            for (byte i = 0; i < (byte)Characters.End; ++i)
            {
                if (i >= _weaponArray.Length)
                {
                    break;
                }
                newArray[i] = _weaponArray[i];
            }
            _weaponArray = newArray;
            _weaponData.SetWeaponArray(newArray);
            EditorUtility.SetDirty(_weaponData);
        }
    }


    public override void OnInspectorGUI()
    {
        for (CharacterWeapons i = 0; i < CharacterWeapons.None; ++i)
        {
            Undo.RecordObject(_weaponData, "Weapon data modified");

            BeginHorizontal();
            LabelField(i.ToString(), EditorStyles.boldLabel, GUILayout.MaxWidth(180.0f));
            if (_weaponArray[(int)i].Image != null)
            {
                GUILayout.Box(_weaponArray[(int)i].Image.texture, GUILayout.MinHeight(5.0f), GUILayout.MinWidth(5.0f), GUILayout.MaxHeight(64.0f), GUILayout.MaxWidth(64.0f));
            }
            if (GUILayout.Button("Update Image", GUILayout.MaxWidth(180.0f)))
            {
                _weaponArray[(int)i].Image = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Textures/WeaponImages/{i.ToString()}.png");
            }
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Damage", GUILayout.MaxWidth(180.0f));
            _weaponArray[(int)i].Damage = (ushort)IntField(_weaponArray[(int)i].Damage);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Range", GUILayout.MaxWidth(180.0f));
            _weaponArray[(int)i].Range = FloatField(_weaponArray[(int)i].Range);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Attack Time", GUILayout.MaxWidth(180.0f));
            _weaponArray[(int)i].AttackTime = FloatField(_weaponArray[(int)i].AttackTime);
            EndHorizontal();

            BeginHorizontal();
            LabelField("  Stock", GUILayout.MaxWidth(180.0f));
            _weaponArray[(int)i].Stock = (ushort)IntField(_weaponArray[(int)i].Stock);
            EndHorizontal();

            Space(20.0f);
        }
    }
}
