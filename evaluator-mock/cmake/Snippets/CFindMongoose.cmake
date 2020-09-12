SET(MONGOOSE_ROOT_PATH ${CMAKE_SOURCE_DIR}/extern/mongoose)
SET(MONGOOSE_INCLUDE_DIRS ${CMAKE_SOURCE_DIR}/extern/mongoose/ ${CMAKE_SOURCE_DIR}/extern/mongoose/mongoose)

MESSAGE("Select mongoose library directory ${CMAKE_SYSTEM_NAME}")
if (MSVC_VERSION EQUAL 1900) # this is Visual Studio 14 2015
	LINK_DIRECTORIES(${CMAKE_SOURCE_DIR}/extern/mongoose/mongoose/lib/${CMAKE_SYSTEM_NAME})
	SET(MONGOOSE_LIBS optimized mongoose debug mongoosed)
else()
	LINK_DIRECTORIES(${CMAKE_SOURCE_DIR}/extern/mongoose/mongoose/lib/${CMAKE_SYSTEM_NAME})
	SET(MONGOOSE_LIBS mongoose)
endif()
SET(MONGOOSE_FOUND ON)