using Godot;
using System;

public partial class Wheat : StaticBody3D
{
	private AnimationTree _anime;
	private AnimationNodeStateMachinePlayback _stateMachine;
	private float _growthTime = 30,
	_growthTimeStart,
	_growthTimeEnd,
	_changeGrowth;
	private float _growthBoost = 1;
	private DayNightCycle _dayNightCycle;
	private Timer _timer;
	public override void _Ready()
	{
		_anime = GetNode<AnimationTree>("AnimationTree");
		_stateMachine = (AnimationNodeStateMachinePlayback)_anime.Get("parameters/playback");
		_dayNightCycle = GetNode<DayNightCycle>("../DayNightCycle");
		_timer = GetNode<Timer>("../LevelTimer");
		_timer.Timeout += OnTimerTimeout;
		_growthTimeStart = _dayNightCycle.leveltime;
		_growthTimeEnd = _growthTimeStart + _growthTime;
	}
	public override void _Process(double delta)
	{
		if (_changeGrowth < _growthTimeEnd)
		{
			Growing();
		}
	}	
	public void Growing() 
	{
		_changeGrowth = _dayNightCycle.leveltime * _growthBoost;
		if (_changeGrowth == _growthTimeStart)
			_stateMachine.Travel("stage_1");
		if (_changeGrowth == (_growthTimeEnd/3))
			_stateMachine.Travel("stage_2");
		if (_changeGrowth == (_growthTimeEnd/3)*2)
			_stateMachine.Travel("stage_3");
		if (_changeGrowth == _growthTimeEnd)
			_stateMachine.Travel("stage_4");
	}
	private void OnTimerTimeout()
	{
		//_changeGrowth++;
	}
}
