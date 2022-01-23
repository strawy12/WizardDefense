using UnityEngine;
using System.Collections;

public enum SlimeType
{
	TYPE_A,
	TYPE_B
}

public enum SlimeAniType
{
	WALK = 1,
	DAMAGE,
	ATTACK_01,
	ATTACK_02,
	END
}

public enum DIE_ANIMATION_STATE
{
	NONE,
	START,
	END
}

public class SlimePackManager : MonoBehaviour {

	public float _rotateEulerAngle = 0f;
	public GameObject[] _orgSlimeTypeA;
	public GameObject[] _orgSlimeTypeB;
	public GameObject[] _helmetTypeA;
	public GameObject[] _helmetTypeB;
	public GameObject[] _boomSlimes;
	public GameObject[] _ghostSlimes;
	public GameObject[] _kingSlimes;
	public GameObject[] _zombieSlimes;
	public GameObject[] _zombieAxeSlimes;
	public GameObject[] _slimeNames;
	public GameObject _uiColorButton;
	public GameObject _uiAttack02Button;
	public GameObject[] _deathParticles;
	
	private GameObject _nowSelectedSlime;
	private SlimeType _nowSlimeType;		// 현재 슬라임 Type.
	private int _slimeIndex;				// 슬라임의 종류 번호.
	private int _colorIndex;				// 슬라임의 색상 번호.
	private string _nowSlimeIdleClipName;
	private Quaternion _originRotation;
	private DIE_ANIMATION_STATE _dieAnimationStete;

	private const int SLIME_COUNT = 7;
	private const int SLIME_COLOR_COUNT = 6;

	void Awake()
	{
		_slimeIndex = _colorIndex = 0;
		_nowSlimeType = SlimeType.TYPE_A;
		_nowSelectedSlime = _orgSlimeTypeA [_colorIndex];
		_originRotation = _nowSelectedSlime.transform.rotation;
		_nowSlimeIdleClipName = null;
		_dieAnimationStete = DIE_ANIMATION_STATE.NONE;
		CheckSlimeUI ();
	}

	void Update()
	{
		_nowSelectedSlime.transform.Rotate (new Vector3(0f, _rotateEulerAngle, 0f) * Time.deltaTime);
		if (_dieAnimationStete == DIE_ANIMATION_STATE.END)
			DieAnimation ();
	}

	void ResetIdleAnimation()
	{
		if (_nowSlimeIdleClipName != null) {
			Animation slimeAni = _nowSelectedSlime.GetComponent<Animation>();
			slimeAni.CrossFade(_nowSlimeIdleClipName);
			_nowSlimeIdleClipName = null;
		}
	}

	void CheckSlimeUI()
	{
		if (_slimeIndex > 1)
			_uiColorButton.SetActive (false);
		else
			_uiColorButton.SetActive (true);

		if (_slimeIndex == 0 || _slimeIndex == 3 || _slimeIndex == 4) 
			_uiAttack02Button.SetActive (false);
		else
			_uiAttack02Button.SetActive (true);
	}

	public void ChangeSlime(BUTTON_TYPE type)
	{
		EndDieAnimation ();
		ResetIdleAnimation ();
		_nowSelectedSlime.transform.rotation = _originRotation;
		_nowSelectedSlime.SetActive (false);
		_slimeNames [_slimeIndex].SetActive (false);
		_nowSlimeType = SlimeType.TYPE_A;
		_colorIndex = 0;
		if (type == BUTTON_TYPE.NEXT)
			_slimeIndex = (_slimeIndex + 1) % SLIME_COUNT;
		else if (type == BUTTON_TYPE.PREV) {
			_slimeIndex = (_slimeIndex == 0) ? SLIME_COUNT - 1 : (_slimeIndex - 1) % SLIME_COUNT;
		}
		CheckSlime ();
	}

	public void ChangeType(SlimeType type)
	{
		EndDieAnimation ();
		ResetIdleAnimation ();
		_nowSelectedSlime.transform.rotation = _originRotation;
		_nowSelectedSlime.SetActive (false);
		_nowSlimeType = type;
		CheckSlime ();
	}

	public void ChangeColor()
	{
		if (_slimeIndex > 1)
			return;
		EndDieAnimation ();
		ResetIdleAnimation ();
		_nowSelectedSlime.transform.rotation = _originRotation;
		_nowSelectedSlime.SetActive (false);
		_colorIndex = (_colorIndex + 1) % SLIME_COLOR_COUNT;
		CheckSlime ();
	}

	public void ChangeAnimation(SlimeAniType aniType)
	{
		EndDieAnimation ();
		Animation slimeAni = _nowSelectedSlime.GetComponent<Animation>();
		int clipCount = slimeAni.GetClipCount ();
		if (aniType == SlimeAniType.ATTACK_02 && clipCount < (int)SlimeAniType.END)
			return;

		string[] aniNames = new string[clipCount];
		int index = 0;
		foreach (AnimationState state in slimeAni) {
			aniNames[index++] = state.clip.name;
		}
		int findIndex = FindNameInString (aniNames, "_Idle");
		if (findIndex == -1) {
			Debug.Log("Error : This Animation has no idle");
			return;
		}
		_nowSlimeIdleClipName = aniNames [findIndex];

		switch(aniType)
		{
		case SlimeAniType.WALK:
			findIndex = FindNameInString(aniNames, "_Walk");
			break;
		case SlimeAniType.DAMAGE:
			findIndex = FindNameInString(aniNames, "_Damage");
			break;
		case SlimeAniType.ATTACK_01:
			findIndex = FindNameInString(aniNames, "_Attack01");
			break;
		case SlimeAniType.ATTACK_02:
			findIndex = FindNameInString(aniNames, "_Attack02");
			break;
		}
		if (findIndex == -1) {
			Debug.Log("Error : This Animation has no clip");
			return;
		}
		slimeAni.CrossFade (aniNames[findIndex]);
	}

	public bool CheckDieAnimation()
	{
		if (_dieAnimationStete != DIE_ANIMATION_STATE.NONE)
			return false;
		return true;
	}

	public void DieAnimation()
	{
		_nowSelectedSlime.SetActive (false);
		if (_slimeIndex <= 1)
			Instantiate (_deathParticles [_colorIndex]);
		else {
			if(_slimeIndex < SLIME_COUNT - 1)
				Instantiate(_deathParticles[_slimeIndex - 2 + SLIME_COLOR_COUNT]);
			else
				Instantiate(_deathParticles[_deathParticles.Length - 1]);
		}
		_dieAnimationStete = DIE_ANIMATION_STATE.START;
		StartCoroutine (DelayEnableObject());
	}

	int FindNameInString(string[] names, string findName)
	{
		for (int i = 0; i < names.Length; i++) {
			if(names[i].EndsWith(findName))
				return i;
		}
		return -1;
	}

	void EndDieAnimation()
	{
		if (_dieAnimationStete != DIE_ANIMATION_STATE.NONE) {
			_dieAnimationStete = DIE_ANIMATION_STATE.NONE;
			_nowSelectedSlime.SetActive(true);
		}
	}

	void CheckSlime()
	{
		switch(_slimeIndex)
		{
		case 0:
			if(_nowSlimeType == SlimeType.TYPE_A)
				_nowSelectedSlime = _orgSlimeTypeA[_colorIndex];
			else
				_nowSelectedSlime = _orgSlimeTypeB[_colorIndex];
			break;
		case 1:
			if(_nowSlimeType == SlimeType.TYPE_A)
				_nowSelectedSlime = _helmetTypeA[_colorIndex];
			else
				_nowSelectedSlime = _helmetTypeB[_colorIndex];
			break;
		case 2:
			_nowSelectedSlime = _boomSlimes[(int)_nowSlimeType];
			break;
		case 3:
			_nowSelectedSlime = _ghostSlimes[(int)_nowSlimeType];
			break;
		case 4:
			_nowSelectedSlime = _kingSlimes[(int)_nowSlimeType];
			break;
		case 5:
			_nowSelectedSlime = _zombieSlimes[(int)_nowSlimeType];
			break;
		case 6:
			_nowSelectedSlime = _zombieAxeSlimes[(int)_nowSlimeType];
			break;
		}
		CheckSlimeUI ();
		_slimeNames [_slimeIndex].SetActive (true);
		_nowSelectedSlime.SetActive (true);
	}

	IEnumerator ChangeIdleAnimation(float time)
	{
		yield return new WaitForSeconds (time);
		if (_nowSlimeIdleClipName != null) {
			Animation slimeAni = _nowSelectedSlime.GetComponent<Animation>();
			slimeAni.CrossFade(_nowSlimeIdleClipName);
			_nowSlimeIdleClipName = null;
		}
	}

	IEnumerator DelayEnableObject()
	{
		yield return new WaitForSeconds(1.0f);
		_nowSelectedSlime.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		if (_dieAnimationStete == DIE_ANIMATION_STATE.START) {
			_dieAnimationStete = DIE_ANIMATION_STATE.END;

		}
	}
}
