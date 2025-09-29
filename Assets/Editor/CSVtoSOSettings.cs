using System.Collections.Generic;
using UnityEngine;

// 프로젝트 메뉴에서 Create > SO Settings > CSVtoSO Settings로 생성 가능
[CreateAssetMenu(fileName = "CSVtoSOSettings", menuName = "SO Settings/CSV to SO Settings")]
public class CSVtoSOSettings : ScriptableObject
{
    public List<string> soTypeNames = new List<string>();
}