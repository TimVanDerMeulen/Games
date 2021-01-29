currentDir="$(pwd)"

########################## CONFIG ##########################

#baseProjectDir="/home/tim/git/privat/Games/godot/rust-games/test/"
baseProjectDir="./"
codeProject="testGameCode"
godotProject="testGame"

########################### SYNC ###########################

cd $baseProjectDir

# build lib
cd $codeProject
cargo build

# go back to base project dir
cd ..

# copy lib to godot
cp $codeProject/target/debug/lib*.so $godotProject/lib

######################### POST SYNC ########################

# go back to call dir
cd $currentDir