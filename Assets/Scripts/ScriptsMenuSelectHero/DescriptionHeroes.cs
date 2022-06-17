using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DescriptionHeroes", menuName = "DescriptionHeroes", order = 0)]
public class DescriptionHeroes : ScriptableObject
{
    public List<DescriptionHero> Description => description;
    [SerializeField]
    private List<DescriptionHero> description;
}

[Serializable]
public struct DescriptionHero
{
    public string Name => name;
    public Sprite ImageHero => imageHero;
    public string HeroDescription => heroDescription;
    public string NameMainAttack => nameMainAttack;
    public Sprite ImageMainAttack => imageMainAttack;
    public string MainAttackDescription => mainAttackDescription;
    public string NameFirstSkill => nameFirstSkill;
    public Sprite ImageFirstSkill => imageFirstSkill;
    public string DescriptionFistSkill => descriptionFirstSkill;
    public string NameSecondSkill => nameSecondSkill;
    public Sprite ImageSecondSkill => imageSecondSkill;
    public string DescriptionSecondSkill => descriptionSecondSkill;


    [SerializeField]
    private string name;
    [SerializeField]
    private Sprite imageHero;
    [SerializeField]
    private string heroDescription;
    [SerializeField]
    private string nameMainAttack;
    [SerializeField]
    private Sprite imageMainAttack;
    [SerializeField]
    private string mainAttackDescription;
    [SerializeField]
    private string nameFirstSkill;
    [SerializeField]
    public Sprite imageFirstSkill;
    [SerializeField]
    private string descriptionFirstSkill;
    [SerializeField]
    private string nameSecondSkill;
    [SerializeField]
    public Sprite imageSecondSkill;
    [SerializeField]
    private string descriptionSecondSkill;

}
