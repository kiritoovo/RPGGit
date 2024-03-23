namespace RPG.Saving{
    public interface ISaveable{
        public object CaptrueState();
        public void RestoreState(object state);
    }
}