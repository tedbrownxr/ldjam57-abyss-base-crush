using DevKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
	public interface IFX 
	{
		bool IsActive();
		void Process();
		void Activate(Vector3 position);
	}
}
