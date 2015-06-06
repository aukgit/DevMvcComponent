namespace DevMVCComponent.Hashing {
    public static class Hasher {
        /// <summary>
        ///     Checks nulls and returns only codes for existing ones.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Get(params object[] o) {
            var output = "";
            for (var i = 0; o[i] != null && i < o.Length; i++) {
                output += o[i].GetHashCode() + "_";
            }
            return output.GetHashCode().ToString();
        }
    }
}