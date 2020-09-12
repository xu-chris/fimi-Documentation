# Find all dependencies. To enable reuse, each package is in it's own file in Snippets
SET(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} ${CMAKE_SOURCE_DIR}/cmake/Modules/)

# Clear
SET(EXTERN_LIBS "")
SET(EXTERN_INCS "")

# Mongoose (Websocket)
INCLUDE(${CMAKE_SOURCE_DIR}/cmake/Snippets/CFindMongoose.cmake)
IF(${MONGOOSE_FOUND})
 ADD_DEFINITIONS(-DWITH_MONGOOSE)
 MESSAGE("Building with Mongoose (Websocket).")
 SET(EXTERN_INCS ${EXTERN_INCS} ${MONGOOSE_INCLUDE_DIRS})
 SET(EXTERN_LIBS ${EXTERN_LIBS} ${MONGOOSE_LIBS})
ENDIF()