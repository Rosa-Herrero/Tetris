using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Tetrominoe
    {
        private static Random rnd = new Random();
        private bool isPortrait = false;
        private int[] pix = new int[2];
        private List<Location> locationList = new List<Location>();
        private readonly Shape tetro;

        public Tetrominoe()
        {
            //PENDENT IMPLEMENTAR
        }

        public List<Location> GetLocation()
        {
            //PENDENT IMPLEMENTAR
        }
        public void draw()
        {
            //PENDENT IMPLEMENTAR
        }
        public void clearScreen()
        {
            //PENDENT IMPLEMENTAR
        }

        public void Spawn()
        {
            //PENDENT IMPLEMENTAR
        }


        /*
         * IMPORTANT: NO TOQUEU EL SEGÜENT CODI
         */

        public void MegaDrop()
        {
            while (!isSomethingBelow())
            {
                for (int numCount = 0; numCount < this.locationList.Count; numCount++)
                {
                    locationList[numCount].moveDown();
                }
            }
        }
        public bool Drop()
        {
            if (isSomethingBelow())
                return true;
            for (int numCount = 0; numCount < this.locationList.Count; numCount++)
            {
                locationList[numCount].moveDown();
            }
            return false;
        }

        public void moveLeft()
        {
            if (!isSomethingLeft())
            {
                for (int i = 0; i < locationList.Count; i++)
                {
                    locationList[i].moveLeft();
                }
            }
        }

        public void moveRight()
        {
            if (!isSomethingRight())
            {
                for (int i = 0; i < locationList.Count; i++)
                {
                    locationList[i].moveRight();
                }
            }
        }

        public void Rotate()
        {
            List<Location> templocation = new List<Location>();
            int center = (Grid.nCOL - tetro.shape.GetLength(1)) / 2;
            for (int i = 0; i < tetro.shape.GetLength(0); i++)
            {
                for (int j = 0; j < tetro.shape.GetLength(1); j++)
                {
                    if (tetro.shape[i, j] == 1)
                    {
                        templocation.Add(new Location(i, center + j));
                    }
                }
            }

            if (tetro.type == ShapeTypes.I)
            {

                if (isPortrait == false)
                {
                    for (int i = 0; i < locationList.Count; i++)
                    {
                        templocation[i] = TransformMatrix(locationList[i], locationList[2], "Clockwise");
                    }
                }
                else
                {
                    for (int i = 0; i < locationList.Count; i++)
                    {
                        templocation[i] = TransformMatrix(locationList[i], locationList[2], "Counterclockwise");
                    }
                }
            }

            else if (tetro.type == ShapeTypes.S)
            {
                for (int i = 0; i < locationList.Count; i++)
                {
                    templocation[i] = TransformMatrix(locationList[i], locationList[3], "Clockwise");
                }
            }

            else if (tetro.type == ShapeTypes.O) return;
            else
            {
                for (int i = 0; i < locationList.Count; i++)
                {
                    templocation[i] = TransformMatrix(locationList[i], locationList[2], "Clockwise");
                }
            }


            for (int count = 0; count < locationList.Count && ( isOverlayLeft(templocation) != false | isOverlayRight(templocation) != false | isOverlayBelow(templocation) != false); count++)
            {
                if (isOverlayLeft(templocation) == true)
                {
                    for (int i = 0; i < locationList.Count; i++)
                    {
                        templocation[i].moveRight();
                    }
                }

                if (isOverlayRight(templocation) == true)
                {
                    for (int i = 0; i < locationList.Count; i++)
                    {
                        templocation[i].moveLeft();
                    }
                }
                if (isOverlayBelow(templocation) == true)
                {
                    for (int i = 0; i < locationList.Count; i++)
                    {
                        templocation[i].moveDown();
                    }
                }
            }

            locationList = templocation;

        }

        private Location TransformMatrix(Location coord, Location axis, string dir)
        {
            Location pcoord = new Location(coord.getRow() - axis.getRow(), coord.getCol() - axis.getCol());
            if (dir == "Counterclockwise")
            {
                pcoord = new Location(-pcoord.getCol(), pcoord.getRow());
            }
            else if (dir == "Clockwise")
            {
                pcoord = new Location(pcoord.getCol(), -pcoord.getRow());
            }

            return new Location(pcoord.getRow() + axis.getRow(), pcoord.getCol() + axis.getCol());
        }
    
        private bool isSomethingBelow()
        {
            foreach(Location loc in locationList)
            {
                if (loc.getRow() + 1 >= Grid.nROW)
                    return true;
                if (Grid.getDropped( loc.getRow() + 1, loc.getCol()) == 1)
                    return true;
            }
            return false;
        }
        private bool? isOverlayBelow(List<Location> newLocations)
        {
            List<int> ycoords = new List<int>();
            foreach (Location loc in newLocations)
            {
                ycoords.Add(loc.getRow());
                if (loc.getRow() >= Grid.nROW)
                    return true;
                if (loc.getRow() < 0)
                    return null;
                if (loc.getCol() < 0)
                    return null;
                if (loc.getCol() >= Grid.nCOL)
                    return null;
            }
            foreach (Location loc in newLocations)
            {
                if (ycoords.Max() - ycoords.Min() == 3)
                {
                    if (ycoords.Max() == loc.getRow() | ycoords.Max() - 1 == loc.getRow())
                    {
                        if (Grid.getDropped(loc.getRow(), loc.getCol()) == 1)
                            return true;
                    }

                }
                else
                {
                    if (ycoords.Max() == loc.getRow())
                    {
                        if (Grid.getDropped(loc.getRow(), loc.getCol()) == 1)
                            return true;
                    }
                }
            }

            return false;
        }


        private bool isSomethingLeft()
        {
            foreach (Location loc in locationList)
            {
                if (loc.getCol() == 0)
                    return true;
                if (Grid.getDropped( loc.getRow(), loc.getCol() - 1) == 1)
                    return true;
            }
            return false;
        }
        private bool? isOverlayLeft(List<Location> newLocations)
        {
            List<int> xcoords = new List<int>();
            foreach (Location loc in newLocations)
            {
                xcoords.Add(loc.getCol());
                if (loc.getCol() < 0)
                    return true;
                if (loc.getCol() >= Grid.nCOL)
                    return false;
                if (loc.getRow() >= Grid.nROW)
                    return null;
                if (loc.getRow() < 0)
                    return null;
            }
            foreach (Location loc in newLocations)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if (xcoords.Min() == loc.getCol() | xcoords.Min() + 1 == loc.getCol())
                    {
                        if (Grid.getDropped( loc.getRow(), loc.getCol()) == 1)
                            return true;
                    }

                }
                else
                {
                    if (xcoords.Min() == loc.getCol())
                    {
                        if (Grid.getDropped(loc.getRow(), loc.getCol()) == 1)
                            return true;
                    }
                }
            }
            return false;
        }
        private bool isSomethingRight()
        {
            foreach (Location loc in locationList)
            {
                if (loc.getCol() == Grid.nCOL-1)
                    return true;
                if (Grid.getDropped(loc.getRow(), loc.getCol() + 1) == 1)
                    return true;
            }
            return false;
        }
        private bool? isOverlayRight(List<Location> newLocations)
        {
            List<int> xcoords = new List<int>();
            foreach (Location loc in newLocations)
            {
                xcoords.Add(loc.getCol());
                if (loc.getCol() >= Grid.nCOL)
                    return true;
                if (loc.getCol() < 0)
                    return false;
                if (loc.getRow() >= Grid.nROW)
                    return null;
                if (loc.getRow() < 0)
                    return null;
            }
            foreach (Location loc in newLocations)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if (xcoords.Max() == loc.getCol() | xcoords.Max() - 1 == loc.getCol())
                    {
                        if (Grid.getDropped(loc.getRow(), loc.getCol()) == 1)
                            return true;
                    }
                }
                else
                {
                    if (xcoords.Max() == loc.getCol())
                    {
                        if (Grid.getDropped(loc.getRow(), loc.getCol()) == 1)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}