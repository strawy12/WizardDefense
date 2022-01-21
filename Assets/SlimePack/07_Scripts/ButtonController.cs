using UnityEngine;
using System.Collections;

public enum BUTTON_TYPE
{
	NEXT,
	PREV,
	TYPE_A,
	TYPE_B,
	COLOR,
	WALK,
	DAMAGE,
	ATTACK_01,
	ATTACK_02,
	DIE
}

public class ButtonController : MonoBehaviour {

	public BUTTON_TYPE _buttonType;
	public SlimePackManager _manager;
	private Vector3 _scale;

	void Start()
	{
		_scale = transform.localScale;
	}

	void OnMouseDown()
	{
		transform.localScale = _scale * 1.1f;
	}

	void OnMouseUp()
	{
		transform.localScale = _scale;
	}

	void OnMouseUpAsButton()
	{
		transform.localScale = _scale;
		CheckButton ();
	}

	void CheckButton()
	{
		switch(_buttonType)
		{
		case BUTTON_TYPE.NEXT:
		case BUTTON_TYPE.PREV:
			_manager.ChangeSlime(_buttonType);
			break;
		case BUTTON_TYPE.TYPE_A:
			_manager.ChangeType(SlimeType.TYPE_A);
			break;
		case BUTTON_TYPE.TYPE_B:
			_manager.ChangeType(SlimeType.TYPE_B);
			break;
		case BUTTON_TYPE.COLOR:
			_manager.ChangeColor();
			break;
		case BUTTON_TYPE.WALK:
		case BUTTON_TYPE.DAMAGE:
		case BUTTON_TYPE.ATTACK_01:
		case BUTTON_TYPE.ATTACK_02:
		{
			SlimeAniType type = (SlimeAniType)((int)_buttonType - (int)BUTTON_TYPE.WALK + 1);
			_manager.ChangeAnimation(type);
		}
			break;
		case BUTTON_TYPE.DIE:
			if(_manager.CheckDieAnimation())
				_manager.DieAnimation();
			break;;
		}
	}
}
