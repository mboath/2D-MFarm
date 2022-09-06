using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    
    private Season gameSeason;
    
    private int monthInSeason;

    public bool gameClockPause;

    private float tikTime;

    private void Awake()
    {
        NewGameTime();
    }

    private void Start()
    {
        //������OnEnable��ע�ᣬAwake����OnEnableִ�У����Ժ����¼�Ҫ��Start����Awake���Ա��ⷽ����δע��
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        EventHandler.CallGameHourEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;

            if (tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }

            //��סT������ʱ�䣨ֱ������һ���ӣ�
            if (Input.GetKey(KeyCode.T))
            {
                for (int i = 0; i < Settings.secondHold + 1; i++)
                {
                    UpdateGameTime();
                }
            }
        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 6;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 1;
        gameSeason = Season.����;
        monthInSeason = Settings.monthInSeason;
    }

    private void UpdateGameTime()
    {
        gameSecond++;

        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Settings.dayHold)
                    {
                        gameMonth++;
                        monthInSeason--;
                        gameDay = 1;
                        
                        if (monthInSeason == 0)
                        {
                            monthInSeason = Settings.monthInSeason;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if (seasonNumber > Settings.seasonHold)
                            {
                                gameYear++;
                                seasonNumber = 0;
                                gameMonth = 1;
                            }   //�����

                            gameSeason = (Season)seasonNumber;
                        }   //���ڸ���
                    }   //�¸���
                }   //�����
                EventHandler.CallGameHourEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }   //Сʱ����
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        }   //���Ӹ���

        //Debug.Log("Second: " + gameSecond + " Minute: " + gameMinute + " Hour: " + gameHour + " Day: " + gameDay + " Month: " + gameMonth + " Season: " + gameSeason + " Year: " + gameYear);
    }
}
