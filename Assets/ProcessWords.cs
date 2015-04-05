using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProcessWords : MonoBehaviour {

	public TextAsset ta;

	// Use this for initialization
	void Start () {
		StartCoroutine("DoProcessWords");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//qwerty
	private string keyboard = "qwertyuiopasdfghjkl;zxcvbnm,./";
	//azerty	
	//private string keyboard = "azertyuiopqsdfghjkl;wxcvbnm,./";
	//dvorak
	private string dvorakkeyboard = "'<>pyfgcrlaoeuidhtns;qjkxbmwvz";

	private int[] keyIndex;
	private int[] keyIndexDvorak;

    private char TranslateChar(char s, int dx, int dy){
		int index = keyIndex [s];
		int ix = index % 10;
		int iy = index / 10;
		ix += dx;
		iy += dy;
		if (ix < 0 || ix >= 10 || iy < 0 || iy >= 3) {
			return '0';
		}
		return keyboard [ix + iy * 10];
	}
	
	private string TranslateWordToDvorak(string s){
		System.Text.StringBuilder newword = new System.Text.StringBuilder (s.Length);
		for (int i=0; i<s.Length; i++) {
			var newc = dvorakkeyboard[keyIndex[s[i]]];
			newword.Append(newc);
        }
        return newword.ToString();
	}
	
	private string TranslateWordFromDvorak(string s){
		System.Text.StringBuilder newword = new System.Text.StringBuilder (s.Length);
		for (int i=0; i<s.Length; i++) {
			var newc = keyboard[keyIndexDvorak[s[i]]];
			newword.Append(newc);
		}
		return newword.ToString();
    }
    
    
    private string TranslateWord(string s, int dx, int dy){
		System.Text.StringBuilder newword = new System.Text.StringBuilder (s.Length);
		for (int i=0; i<s.Length; i++) {
			var newc = TranslateChar(s[i],dx,dy);
			if (newc=='0'){
				return null;
			}
			newword.Append(newc);
		}
		return newword.ToString();
	}

	public IEnumerator DoProcessWords(){
		keyIndex = new int[256];
		for (int i=0;i<keyboard.Length;i++){
			keyIndex[keyboard[i]]=i;
		}
		print("calculated key index");
		yield return 0;
		var words = ta.text.Split(new [] { '\r', '\n' },System.StringSplitOptions.RemoveEmptyEntries);

		var longest = words.Max(w=>w.Length);
		HashSet<string>[] strings = new HashSet<string>[longest+1];
		for(var i=0;i<strings.Length;i++){
			strings[i] = new HashSet<string>();
		}

		for(var i=0;i<words.Length;i++){
			var w = words[i];
			strings[w.Length].Add(w);
		}
		
		print("populated hashset");
		yield return 0;
		
		string result = "";
		for (var i=strings.Length-1;i>=1;i--){//strings.Length;i++){
			result+="\n\nwords of length "+i+"\n";
			var stringset = strings[i];  
			foreach(var s in stringset){
//				for (int dx=0;dx<9;dx++){
//					for (int dy=-2;dy<3;dy++){
//						if (dx==0&&dy==0){
//							continue;
//						}
						
						var w = TranslateWordToDvorak(s);
						if (w!=s&&w!=null&&stringset.Contains(w)){
							result += "\n"+s+" -> " + w;
						}

            }        

		}

		yield return 0;
			
		print (result);
		print ("done");
		yield break;
	}
}
