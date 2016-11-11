using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CZ_Character : ScriptableObject {

    public string characterName;

    public List<CZ_Section> sections = new List<CZ_Section>();

    public List<CZ_Variable> variables = new List<CZ_Variable>();

    public GameObject baseModel;
	
}
