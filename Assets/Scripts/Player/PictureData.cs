using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Just an info holder for pictures
public class PictureData{
	//Names of creatures in this photo
	public List<string> namesOfCreatures;
	//Score aspects for this photo
	public List<Dictionary<string, float>> scoringCriteria;	
	
	public PictureData(List<BasicCreature> creaturesInPicture, List<Dictionary<string, float>> _score){
		namesOfCreatures = new List<string>();
		if(creaturesInPicture!=null){
			foreach(BasicCreature c in creaturesInPicture){
				namesOfCreatures.Add(c.GetType().Name);
			}
			scoringCriteria = _score;
		}
	}
}
