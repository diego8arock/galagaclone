using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine.UI;
using System.Linq;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        public GameObject PrefaBee;
        public GameObject PrefaButterfly;
        public GameObject PrefaBoss;
        public GameObject Ship;
        public GameObject Player;
        public float VelocityAllien;
        public static List<Allien> Alliens { set; get; }
        private bool Started = false;
        private Vector3 InitialPosition;
        public static List<GameObject> Lives;
        private Text StartText;
        private Text StageText;
        private Text LiveText;
        private Text ReadyText;
        private Text GameOverText;
        public float TimeLiveEnable = 0.3f;
        public static int ScoreValue;
        public static int HighScoreValue;
        private float y = 3.7f;
        public float LimitRight;
        public float LimitLeft;
        public float TilesToMove;
        public float TimerToMove;
        public float TimerToAlienInvasion = 17.0f;
        public bool ShowGrid = false;
        public GameObject Grid;
        private bool _beginLevel1 = false;
        public float LimitRightTiles;
        public float LimitLeftTiles;
        public float IndiviudalTilesToMove;

        private Dictionary<string, GameObject> _grid;
        private Dictionary<int, bool> _waves;
        private List<Allien> _alliens;
        private bool _goRight;
        private bool _goLeft;

        private float _staticTimerToMove;
        private bool _alienInvasionCompleted;
        private Vector3 _gridStartPosition;
        private int _multiplier;
        private int _multiplierRight;
        private int _multiplierLeft;

        List<GameObject> _tilesRigth;
        private Vector3 _tileRght0StartPosition;

        List<GameObject> _tilesLeft;
        private Vector3 _tileLeft0StartPosition;

        // Use this for initialization
        void Start()
        {
            ScoreValue = 0;
            HighScoreValue = 30000;
            Alliens = new List<Allien>();
            Lives = new List<GameObject>();
            StartText = GameObject.Find("StartText").GetComponent<Text>();
            StageText = GameObject.Find("StageText").GetComponent<Text>();
            LiveText = GameObject.Find("LiveText").GetComponent<Text>();
            ReadyText = GameObject.Find("ReadyText").GetComponent<Text>();
            GameOverText = GameObject.Find("GameOverText").GetComponent<Text>();

            GameObject ship = (GameObject)Instantiate(Ship, new Vector3(4.58f, -0.53f, 0), Quaternion.identity);
            Lives.Add(ship);
            ship = (GameObject)Instantiate(Ship, new Vector3(5.27f, -0.53f, 0), Quaternion.identity);
            Lives.Add(ship);
            ship = (GameObject)Instantiate(Ship, new Vector3(5.94f, -0.53f, 0), Quaternion.identity);
            Lives.Add(ship);

            StageText.enabled = false;
            ReadyText.enabled = false;
            GameOverText.enabled = false;
            Player.SetActive(false);
            GetGrid();
            Level1Config();
            StartCoroutine(BeginStart());
        }

        IEnumerator BeginStart()
        {
            yield return new WaitForSeconds(1);
            StartText.enabled = false;
            yield return new WaitForSeconds(1);
            StageText.enabled = true;
            yield return new WaitForSeconds(2);
            StageText.enabled = false;
            ReadyText.enabled = true;
            Started = true;
            yield return new WaitForSeconds(2);
            ReadyText.enabled = false;
            StartGame();
        }

        public void GetGrid()
        {
            _grid = new Dictionary<string, GameObject>();

            int level = 1;
            for (int i = 1; i <= 10; i++)
                _grid.Add(level + "-" + i, GameObject.Find(level + "-" + i));

            level = 2;
            for (int i = 1; i <= 10; i++)
                _grid.Add(level + "-" + i, GameObject.Find(level + "-" + i));

            level = 3;
            for (int i = 1; i <= 8; i++)
                _grid.Add(level + "-" + i, GameObject.Find(level + "-" + i));

            level = 4;
            for (int i = 1; i <= 8; i++)
                _grid.Add(level + "-" + i, GameObject.Find(level + "-" + i));

            level = 5;
            for (int i = 1; i <= 4; i++)
                _grid.Add(level + "-" + i, GameObject.Find(level + "-" + i));

            if (!ShowGrid)
            {
                foreach (var key in _grid.Keys)
                {
                    _grid[key].GetComponent<Renderer>().enabled = false;
                }
            }
        }

        void StartGame()
        {
            GetLive(this.Player);
            _beginLevel1 = true;
        }

        public static void GetLive(GameObject Player)
        {
            Lives[Lives.Count - 1].SetActive(false);
            Lives.RemoveAt(Lives.Count - 1);
            Player.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            TimeLiveEnable -= Time.deltaTime;
            if (Started && TimeLiveEnable <= 0)
            {
                LiveText.enabled = !LiveText.enabled;
                TimeLiveEnable = 0.3f;
            }

            if(_beginLevel1)
            {
                Level1Start();
            }
        }

        public static void setScore(GameObject allienObject)
        {
            Text Score = GameObject.Find("Score").GetComponent<Text>();
            Text HighScore = GameObject.Find("HighScore").GetComponent<Text>();
            foreach (Model.Allien allien in GameController.Alliens)
            {
                if (allienObject.Equals(allien.gameObject))
                {
                    GameController.ScoreValue += allien.GetAllienValue();
                    break;
                }
            }
            Score.text = GameController.ScoreValue.ToString();
            if (GameController.ScoreValue > GameController.HighScoreValue)
            {
                GameController.HighScoreValue = GameController.ScoreValue;
                HighScore.text = GameController.ScoreValue.ToString();
            }
        }

        void Level1Start()
        {
            MoveGrid();

            if (!_alienInvasionCompleted)
            {
                TimerToAlienInvasion -= Time.deltaTime;
                if (TimerToAlienInvasion < 15.0f && !_waves[1])
                {
                    GenerateFirstWave();
                    _waves[1] = true;
                }
                if (TimerToAlienInvasion < 11.0f && !_waves[2])
                {
                    GenerateSecondWave();
                    _waves[2] = true;
                }
                if (TimerToAlienInvasion < 7.0f && !_waves[3])
                {
                    GenerateThirdWave();
                    _waves[3] = true;
                }
                if (TimerToAlienInvasion < 3.0f && !_waves[4])
                {
                    GenerateFourthWave();
                    _waves[4] = true;
                }
                if (TimerToAlienInvasion < 0.0f && !_waves[5])
                {
                    GenerateFifthWave();
                    _waves[5] = true;
                }
            }

            if (_waves[5] && !_alienInvasionCompleted)
                _alienInvasionCompleted = AreAllAliensInGrid();
        }

        void Level1Config()
        {
            _waves = new Dictionary<int, bool>();
            _waves.Add(1, false);
            _waves.Add(2, false);
            _waves.Add(3, false);
            _waves.Add(4, false);
            _waves.Add(5, false);

            _alliens = new List<Allien>();

            _goRight = true;
            _goLeft = false;
            _staticTimerToMove = TimerToMove;
            _alienInvasionCompleted = false;
            _gridStartPosition = Grid.transform.position;

            _tilesRigth = _grid.Where(x => x.Key.Contains("1-") && int.Parse(x.Key.Split('-')[1]) >= 6).Select(y => y.Value).ToList();
            _tilesRigth.AddRange(_grid.Where(x => x.Key.Contains("2-") && int.Parse(x.Key.Split('-')[1]) >= 6).Select(y => y.Value).ToList());
            _tilesRigth.AddRange(_grid.Where(x => x.Key.Contains("3-") && int.Parse(x.Key.Split('-')[1]) >= 5).Select(y => y.Value).ToList());
            _tilesRigth.AddRange(_grid.Where(x => x.Key.Contains("4-") && int.Parse(x.Key.Split('-')[1]) >= 5).Select(y => y.Value).ToList());

            _tilesLeft = _grid.Where(x => x.Key.Contains("1-") && int.Parse(x.Key.Split('-')[1]) <= 5).Select(y => y.Value).ToList();
            _tilesLeft.AddRange(_grid.Where(x => x.Key.Contains("2-") && int.Parse(x.Key.Split('-')[1]) <= 5).Select(y => y.Value).ToList());
            _tilesLeft.AddRange(_grid.Where(x => x.Key.Contains("3-") && int.Parse(x.Key.Split('-')[1]) <= 4).Select(y => y.Value).ToList());
            _tilesLeft.AddRange(_grid.Where(x => x.Key.Contains("4-") && int.Parse(x.Key.Split('-')[1]) <= 4).Select(y => y.Value).ToList());

            _tileRght0StartPosition = _tilesRigth[0].transform.position;
            _tileLeft0StartPosition = _tilesLeft[0].transform.position;

            _multiplier = 1;
            _multiplierLeft = -1;
            _multiplierRight = 1;
        }

        void MoveGrid()
        {
            if (!_alienInvasionCompleted)
                GridDiscretMovement();
            else
            {
                if (Grid.transform.position != _gridStartPosition)
                    GridDiscretMovement();
                else
                {
                    GridSpaceingMovement();
                }
            }
        }

        void GridDiscretMovement()
        {
            if (TimerToMove <= 0)
            {
                Grid.transform.position = new Vector3(Grid.transform.position.x + (_multiplier * TilesToMove), Grid.transform.position.y);

                if (Grid.transform.position.x >= LimitRight)
                    _multiplier = -1;

                if (Grid.transform.position.x <= LimitLeft)
                    _multiplier = 1;

                TimerToMove = _staticTimerToMove;
            }

            TimerToMove -= Time.deltaTime;
        }

        void GridSpaceingMovement()
        {
            if (TimerToMove <= 0)
            {
                TileDiscretMovementRight(_tilesRigth, _tileRght0StartPosition.x + LimitRightTiles, _tileRght0StartPosition.x);
                TileDiscretMovementLeft(_tilesLeft, _tileLeft0StartPosition.x, _tileLeft0StartPosition.x - LimitLeftTiles);
                TimerToMove = _staticTimerToMove;
            }
            TimerToMove -= Time.deltaTime;
        }

        void TileDiscretMovementRight(List<GameObject> tiles, float limitRight, float limitLeft)
        {
            tiles.ForEach(tile => tile.transform.position = new Vector3(tile.transform.position.x + (_multiplierRight * IndiviudalTilesToMove), tile.transform.position.y));

            if (tiles[0].transform.position.x >= limitRight)
                _multiplierRight = -1;

            if (tiles[0].transform.position.x <= limitLeft)
                _multiplierRight = 1;
        }

        void TileDiscretMovementLeft(List<GameObject> tiles, float limitRight, float limitLeft)
        {
            tiles.ForEach(tile => tile.transform.position = new Vector3(tile.transform.position.x + (_multiplierLeft * IndiviudalTilesToMove), tile.transform.position.y));

            if (tiles[0].transform.position.x >= limitRight)
                _multiplierLeft = -1;

            if (tiles[0].transform.position.x <= limitLeft)
                _multiplierLeft = 1;
        }

        void GenerateFirstWave()
        {
            Vector3 initialPositionFirstWave = new Vector3(0.0f, 6.0f, 0);
            List<GameObject> wave = new List<GameObject> { PrefaBee };
            List<string> tilesId = new List<string> { "1-5", "1-6", "2-5", "2-6" };
            GenerateAlienWave(wave, initialPositionFirstWave, tilesId, "EnterPath1", 2);

            wave = new List<GameObject> { PrefaButterfly };
            tilesId = new List<string> { "3-4", "3-5", "4-4", "4-5" };
            GenerateAlienWave(wave, initialPositionFirstWave, tilesId, "EnterPath2", 2);
        }

        void GenerateSecondWave()
        {
            Vector3 initialPositionSecondWave = new Vector3(-10.0f, -3.0f, 0);
            List<GameObject> wave = new List<GameObject> { PrefaBoss, PrefaButterfly };
            List<string> tilesId = new List<string> { "5-1", "3-3", "5-2", "4-3", "5-3", "3-6", "5-4", "4-6" };
            GenerateAlienWave(wave, initialPositionSecondWave, tilesId, "EnterPath3", 2, true);
        }

        void GenerateThirdWave()
        {
            Vector3 initialPositionThirdWave = new Vector3(10.0f, -3.0f, 0);
            List<GameObject> wave = new List<GameObject> { PrefaButterfly };
            List<string> tilesId = new List<string> { "3-1", "3-2", "3-7", "3-8", "4-1", "4-2", "4-7", "4-8" };
            GenerateAlienWave(wave, initialPositionThirdWave, tilesId, "EnterPath4", 2);
        }

        void GenerateFourthWave()
        {
            Vector3 initialPositionFourthWave = new Vector3(0.0f, 6.0f, 0);
            List<GameObject> wave = new List<GameObject> { PrefaBee };
            List<string> tilesId = new List<string> { "1-1", "2-1", "1-2", "2-2", "1-3", "2-3", "1-4", "2-4" };
            GenerateAlienWave(wave, initialPositionFourthWave, tilesId, "EnterPath5", 2);
        }

        void GenerateFifthWave()
        {
            Vector3 initialPositionFifthWave = new Vector3(0.0f, 6.0f, 0);
            List<GameObject> wave = new List<GameObject> { PrefaBee };
            List<string> tilesId = new List<string> { "1-7", "2-7", "1-8", "2-8", "1-9", "2-9", "1-10", "2-10" };
            GenerateAlienWave(wave, initialPositionFifthWave, tilesId, "EnterPath6", 2);
        }

        public Allien AddAllien(GameObject gameObject, float velocity, string tileName)
        {
            Allien allien = new Allien();
            allien.gameObject = gameObject;
            allien.Velocity = velocity;
            allien.TileName = tileName;
            allien.state = Model.AllienState.CREATED;
            return allien;
        }

        void GenerateAlienWave(List<GameObject> alliensPrefab, Vector3 initialPosition, List<string> tilesId, string path, float time, bool intercalate = false)
        {
            float delay = 0.0f;
            int position = 0;
            foreach (string tileId in tilesId)
            {
                GameObject allienGameObject = null;
                allienGameObject = alliensPrefab[position];
                Allien allien = AddAllien((GameObject)Instantiate(allienGameObject, initialPosition, Quaternion.identity), VelocityAllien, tileId);
                _alliens.Add(allien);
                MoveAlien(allien, path, time, ref delay);
                if (intercalate)
                    position = position == 0 ? 1 : 0;
            }
        }

        void MoveAlien(Allien allien, string path, float time, ref float delay)
        {
            delay += 0.1f;
            allien.state = Model.AllienState.ENTERING;
            object[] fparams = new object[] { allien, _grid[allien.TileName] };
            iTween.MoveTo(allien.gameObject, iTween.Hash(
                    "path", iTweenPath.GetPath(path),
                    "time", time,
                    "easetype", iTween.EaseType.linear,
                    "delay", delay,
                    "oncomplete", "SetTileToAlien",
                    "oncompletetarget", allien.gameObject,
                    "oncompleteparams", fparams));
        }

        bool AreAllAliensInGrid()
        {
            return _alliens.Count(a => a.state == Model.AllienState.STILL_IN_GRID) == _alliens.Count;
        }
    }
}
